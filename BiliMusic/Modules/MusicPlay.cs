using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using BiliMusic.Models.Main;
using Microsoft.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.ComponentModel;
using BiliMusic.Helpers;
using BiliMusic.Models;
using Windows.Devices.Enumeration;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Devices;
using Windows.Media.Playback;
using Microsoft.Toolkit.Uwp.Helpers;

namespace BiliMusic.Modules
{
    public class MusicPlay : INotifyPropertyChanged
    {
        public ObservableCollection<PlayModel> playList { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private PlayModel _playInfo = new PlayModel()
        {
            title = "哔哩哔哩 (゜-゜)つロ 干杯~",
            pic = "ms-appx:///Assets/StoreLogo.scale-100.png"
        };
        public PlayModel playInfo
        {
            get { return _playInfo; }
            set
            {
                _playInfo = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("playInfo"));
                    });
                }
            }
        }

        private bool _loading = false;
        public bool loading
        {
            get { return _loading; }
            set
            {
                _loading = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("loading"));
                    });
                }
            }
        }

        private bool _showPasue = false;
        public bool showPasue
        {
            get { return _showPasue; }
            set
            {
                _showPasue = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("showPasue"));
                    });
                }
            }
        }
        private bool _showPlay = false;
        public bool showPlay
        {
            get { return _showPlay; }
            set
            {
                _showPlay = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("showPlay"));
                    });
                }
            }
        }

        private double _bufferingProgress = 0;
        public double bufferingProgress
        {
            get { return _bufferingProgress; }
            set
            {
                _bufferingProgress = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("bufferingProgress"));
                    });
                }
            }
        }

        private double _totalDuration = 0;
        public double totalDuration
        {
            get { return _totalDuration; }
            set
            {
                _totalDuration = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("totalDuration"));
                    });
                }
            }
        }

        private double _position = 0;
        public double position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("position"));
                    });
                }
            }
        }

        private string _durationStr = "00:00 / 00:00";
        public string durationStr
        {
            get { return _durationStr; }
            set
            {
                _durationStr = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("durationStr"));
                    });
                }
            }
        }

        public MediaPlayer mediaPlayer;
        public MediaPlaybackList mediaPlaybackList;
        public MusicPlay()
        {
            playList = new ObservableCollection<PlayModel>();
            mediaPlayer = new MediaPlayer();
            mediaPlayer.AutoPlay = true;
            mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
            mediaPlayer.CommandManager.IsEnabled = true;
            mediaPlayer.PlaybackSession.BufferingEnded += PlaybackSession_BufferingEnded;
            mediaPlayer.PlaybackSession.BufferingStarted += PlaybackSession_BufferingStarted;
            mediaPlayer.PlaybackSession.DownloadProgressChanged += PlaybackSession_DownloadProgressChanged;
            mediaPlayer.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
            mediaPlayer.PlaybackSession.NaturalDurationChanged += PlaybackSession_NaturalDurationChanged;
            mediaPlayer.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;
            mediaPlayer.MediaFailed += MediaPlayer_MediaFailed;

            mediaPlaybackList = new MediaPlaybackList();
            mediaPlaybackList.CurrentItemChanged += MediaPlaybackList_CurrentItemChanged;
            mediaPlaybackList.ItemFailed += MediaPlaybackList_ItemFailed;
            mediaPlaybackList.ItemOpened += MediaPlaybackList_ItemOpened;
            mediaPlayer.Source = mediaPlaybackList;

        }

        private void MediaPlaybackList_ItemOpened(MediaPlaybackList sender, MediaPlaybackItemOpenedEventArgs args)
        {

        }

        private void MediaPlaybackList_ItemFailed(MediaPlaybackList sender, MediaPlaybackItemFailedEventArgs args)
        {
            var index = sender.Items.IndexOf(args.Item);
            var item = playList[index];
            if (args.Error.ErrorCode == MediaPlaybackItemErrorCode.SourceNotSupportedError)
            {
                Task.Run(async () =>
                {
                    var url = await GetUrl(item.songid);
                    if (url == null)
                    {
                        await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                        {
                            Utils.ShowMessageToast("无法播放 " + item.title);
                        });
                        mediaPlaybackList.Items.Remove(args.Item);
                        playList.Remove(item);
                        return;
                    }
                    item.play_url = new Uri(url.cdns[0]);
                    item.qualities = url.qualities;
                    item.backup_url = url.cdns;
                    mediaPlaybackList.Items.Remove(args.Item);
                    mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(item.play_url)));
                });

            }
            else
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Utils.ShowMessageToast("无法播放 " + item.title);
                });
            }

        }

        private void MediaPlayer_MediaFailed(MediaPlayer sender, MediaPlayerFailedEventArgs args)
        {
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Utils.ShowMessageToast("123");
            });

        }



        private void PlaybackSession_PlaybackStateChanged(MediaPlaybackSession sender, object args)
        {
            switch (sender.PlaybackState)
            {
                case MediaPlaybackState.None:
                    showPasue = false;
                    showPlay = true;
                    break;
                case MediaPlaybackState.Opening:
                    showPasue = false;
                    showPlay = true;
                    break;
                case MediaPlaybackState.Buffering:
                    showPasue = false;
                    showPlay = true;
                    break;
                case MediaPlaybackState.Playing:
                    showPasue = true;
                    showPlay = false;
                    break;
                case MediaPlaybackState.Paused:
                    showPasue = false;
                    showPlay = true;
                    break;
                default:
                    break;
            }
        }

        private void PlaybackSession_DownloadProgressChanged(MediaPlaybackSession sender, object args)
        {
            bufferingProgress = mediaPlayer.PlaybackSession.DownloadProgress * 100;
        }

        private void PlaybackSession_NaturalDurationChanged(MediaPlaybackSession sender, object args)
        {
            totalDuration = sender.NaturalDuration.TotalSeconds;
        }

        private void PlaybackSession_PositionChanged(MediaPlaybackSession sender, object args)
        {
            position = sender.Position.TotalSeconds;
            durationStr = $"{sender.Position.ToString(@"mm\:ss")} / {sender.NaturalDuration.ToString(@"mm\:ss")}";
        }

        private void MediaPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (mediaPlaybackList.Items.Count == 0)
            {
                return;
            }
            if (args.NewItem==null)
            {
                playInfo = null;
                loading = true;
                return;
            }
            playInfo = playList[mediaPlaybackList.Items.IndexOf(args.NewItem)];


            var _systemMediaTransportControls = mediaPlayer.SystemMediaTransportControls;
            SystemMediaTransportControlsDisplayUpdater updater = _systemMediaTransportControls.DisplayUpdater;
            updater.Type = MediaPlaybackType.Music;
            updater.MusicProperties.Artist = playList[mediaPlaybackList.Items.IndexOf(args.NewItem)].author;
            updater.MusicProperties.Title = playList[mediaPlaybackList.Items.IndexOf(args.NewItem)].title;
            updater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(playList[Convert.ToInt32(mediaPlaybackList.CurrentItemIndex)].pic));
            updater.Update();
            loading = false;
        }


        private void PlaybackSession_BufferingEnded(MediaPlaybackSession sender, object args)
        {
            loading = false;
        }

        private void PlaybackSession_BufferingStarted(MediaPlaybackSession sender, object args)
        {
            loading = true;
        }

        public void AddPlay(PlayModel play)
        {
            playList.Add(play);
            mediaPlaybackList.Items.Add(new MediaPlaybackItem(MediaSource.CreateFromUri(play.play_url)));

        }


        public async Task<PlayModel> LoadMusicInfo(int songid)
        {
            try
            {
                loading = true;
                var re = await Api.SongDetail(songid).Request();
                if (!re.status)
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        Utils.ShowMessageToast(re.message);
                    });
                    return null;
                }
                var data = re.GetJson<ApiParseModel<SongsDetailModel>>();
                if (data.code != 0)
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        Utils.ShowMessageToast(data.msg + data.message);
                    });
                    return null;
                }

                var url = await GetUrl(songid);
                if (url == null)
                {
                    return null;
                }
                var m = new PlayModel()
                {
                    author = data.data.author,
                    backup_url = url.cdns,
                    is_collect = data.data.is_collect == 1,
                    is_preview = url.type == -1,
                    lyric_url = data.data.lyric_url,
                    pic = data.data.cover_url + "@300w_300h_1e_1c.jpg",
                    play_url = new Uri(url.cdns[0]),
                    qualities = url.qualities,
                    songid = data.data.id,
                    title = data.data.title + " - " + data.data.author,
                    songinfo = data.data
                };
                return m;
            }
            catch (Exception ex)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Utils.ShowMessageToast("读取歌曲信息失败");
                });
                LogHelper.Log("读取歌曲信息失败" + songid, LogType.ERROR, ex);
                return null;
            }
            finally
            {
                loading = false;
            }

        }

        public async Task<SongsUrlModel> GetUrl(int songid)
        {
            try
            {
                var q = SettingHelper.StorageHelper.Read<int>(SettingHelper.DefaultQualities, 2);
                var re = await Api.SongUrl(songid, q).Request();
                if (!re.status)
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                     {
                         Utils.ShowMessageToast(re.message);
                     });
                    return null;
                }
                var data = re.GetJson<ApiParseModel<SongsUrlModel>>();
                if (data.code != 0)
                {
                    await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        Utils.ShowMessageToast(data.msg + data.message);
                    });
                    return null;
                }
                return data.data;
            }
            catch (Exception ex)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Utils.ShowMessageToast("读取歌曲地址失败");
                });
                LogHelper.Log("读取歌曲地址失败" + songid, LogType.ERROR, ex);
                return null;
            }

        }


        /// <summary>
        /// 读取可用的音频设备
        /// </summary>
        /// <returns></returns>
        public async static Task<DeviceInformationCollection> GetAudioRenderDevice()
        {
            string audioSelector = MediaDevice.GetAudioRenderSelector();
            var outputDevices = await DeviceInformation.FindAllAsync(audioSelector);
            return outputDevices;
        }
    }
}
