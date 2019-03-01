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
using Windows.UI.Xaml.Media;
using BiliMusic.Controls;
using System.Diagnostics;

namespace BiliMusic.Modules
{
    /// <summary>
    /// 播放模式
    /// </summary>
    public enum MusicPlayMode
    {
        /// <summary>
        /// 顺序播放
        /// </summary>
        Sequence=0,
        /// <summary>
        /// 列表循环
        /// </summary>
        Loop = 1,
        /// <summary>
        /// 随机播放
        /// </summary>
        Shuffle = 2,
        /// <summary>
        /// 单曲循环
        /// </summary>
        Single = 3
    }
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

        private MusicPlayMode _musicPlayMode =  MusicPlayMode.Loop;
        public MusicPlayMode musicPlayMode
        {
            get { return _musicPlayMode; }
            set
            {
                _musicPlayMode = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("musicPlayMode"));
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

        private double _volume = 100;
        public double volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                if (PropertyChanged != null)
                {
                    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("volume"));
                    });
                }
            }
        }



        public MediaPlayer mediaPlayer;
        public MediaPlaybackList mediaPlaybackList;
        public MusicPlay()
        {
            playList = new ObservableCollection<PlayModel>();
            //播放器
            mediaPlayer = new MediaPlayer();
            mediaPlayer.AutoPlay = true;
            mediaPlayer.AudioCategory = MediaPlayerAudioCategory.Media;
            mediaPlayer.CommandManager.IsEnabled = true;
            //状态变更
            mediaPlayer.PlaybackSession.BufferingEnded += PlaybackSession_BufferingEnded;
            mediaPlayer.PlaybackSession.BufferingStarted += PlaybackSession_BufferingStarted;
            mediaPlayer.PlaybackSession.DownloadProgressChanged += PlaybackSession_DownloadProgressChanged;
            mediaPlayer.PlaybackSession.PositionChanged += PlaybackSession_PositionChanged;
            mediaPlayer.PlaybackSession.NaturalDurationChanged += PlaybackSession_NaturalDurationChanged;
            mediaPlayer.PlaybackSession.PlaybackStateChanged += PlaybackSession_PlaybackStateChanged;

            //播放列表
            mediaPlaybackList = new MediaPlaybackList();
            mediaPlaybackList.CurrentItemChanged += MediaPlaybackList_CurrentItemChanged;
            mediaPlaybackList.ItemFailed += MediaPlaybackList_ItemFailed;
            mediaPlaybackList.ItemOpened += MediaPlaybackList_ItemOpened;

            //登录后测试
            MessageCenter.Logined += MessageCenter_Logined;

            //读取播放模式
            var mode= SettingHelper.StorageHelper.Read<int>(SettingHelper.PlayMode, 1);
            musicPlayMode = (MusicPlayMode)mode;
            SetMusicPlayMode(false);

           
            mediaPlayer.Source = mediaPlaybackList;
        }

        private void MessageCenter_Logined(object sender, object e)
        {
            if (mediaPlaybackList.Items.Count==0)
            {
                return;
            }
            PlayModel[] list = new PlayModel[playList.Count];
            playList.CopyTo(list, 0);
            
            var now = playInfo;

            ReplacePlayList(list);
            var id = now.songid;
            var item = mediaPlaybackList.Items.FirstOrDefault(x => x.Source.CustomProperties["id"].Equals(id));
            mediaPlaybackList.MoveTo(Convert.ToUInt32(mediaPlaybackList.Items.IndexOf(item)));
        }


        /// <summary>
        /// 变更播放模式
        /// </summary>
        public void ChangeMusicPlayMode()
        {
            int now = (int)musicPlayMode;
            if (now==3)
            {
                musicPlayMode = MusicPlayMode.Sequence;
            }
            else
            {
                musicPlayMode = (MusicPlayMode)now+1;
            }

            SetMusicPlayMode(true);
        }
        /// <summary>
        /// 设置播放模式
        /// </summary>
        /// <param name="show"></param>
        private void SetMusicPlayMode(bool show)
        {
            switch (musicPlayMode)
            {
                case MusicPlayMode.Sequence:
                    mediaPlaybackList.AutoRepeatEnabled = false;
                    mediaPlaybackList.ShuffleEnabled = false;
                    break;
                case MusicPlayMode.Loop:
                    mediaPlaybackList.AutoRepeatEnabled = true;
                    mediaPlaybackList.ShuffleEnabled = false;
                    break;
                case MusicPlayMode.Shuffle:
                    mediaPlaybackList.AutoRepeatEnabled = true;
                    mediaPlaybackList.ShuffleEnabled = true;
                    break;
                case MusicPlayMode.Single:
                    mediaPlaybackList.AutoRepeatEnabled = true;
                    mediaPlaybackList.ShuffleEnabled = true;
                    var list = new List<MediaPlaybackItem>();
                    foreach (var item in mediaPlaybackList.Items)
                    {
                        list.Add(mediaPlaybackList.CurrentItem);
                    }
                    mediaPlaybackList.SetShuffledItems(list);
                    break;
                default:
                    break;
            }
            SettingHelper.StorageHelper.Save(SettingHelper.PlayMode,(int)musicPlayMode);
            if (!show)
            {
                return;
            }
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                switch (musicPlayMode)
                {
                    case MusicPlayMode.Sequence:
                        Utils.ShowMessageToast("顺序播放");
                        break;
                    case MusicPlayMode.Loop:
                        Utils.ShowMessageToast("列表循环");
                        break;
                    case MusicPlayMode.Shuffle:
                        Utils.ShowMessageToast("随机播放");
                        break;
                    case MusicPlayMode.Single:
                        Utils.ShowMessageToast("单曲循环");
                        break;
                    default:
                        break;
                }
                
            });
        }

        private void MediaPlaybackList_ItemOpened(MediaPlaybackList sender, MediaPlaybackItemOpenedEventArgs args)
        {

        }

        private void MediaPlaybackList_ItemFailed(MediaPlaybackList sender, MediaPlaybackItemFailedEventArgs args)
        {
            var index = sender.Items.IndexOf(args.Item);
            var item = playList[index];
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Utils.ShowMessageToast("无法播放 " + item.title);
            });
            //var index = sender.Items.IndexOf(args.Item);
            //var item = playList[index];
            //if (args.Error.ErrorCode == MediaPlaybackItemErrorCode.SourceNotSupportedError)
            //{
            //    Task.Run(async () =>
            //    {
            //        var url = await GetUrl(item.songid);
            //        if (url == null)
            //        {
            //            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            //            {
            //                Utils.ShowMessageToast("无法播放 " + item.title);
            //            });
            //            mediaPlaybackList.Items.Remove(args.Item);
            //            playList.Remove(item);
            //            return;
            //        }
            //        item.play_url = new Uri(url.cdns[0]);
            //        item.qualities = url.qualities;
            //        item.backup_url = url.cdns;
            //        mediaPlaybackList.Items.Remove(args.Item);

            //        mediaPlaybackList.Items.Insert(index, new MediaPlaybackItem(MediaSource.CreateFromUri(item.play_url)));
            //    });

            //}
            //else
            //{
            //    DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            //    {
            //        Utils.ShowMessageToast("无法播放 " + item.title);
            //    });
            //}

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
            if (args.NewItem == null)
            {
                playInfo = null;

                loading = true;
                return;
            }
            if (musicPlayMode== MusicPlayMode.Single)
            {
                SetMusicPlayMode(false);
            }


            //重置播放信息
            bufferingProgress = 0;
            this.totalDuration = 0;
            this.position = 0;
            this.durationStr = "00:00 / 00:00";
            var songid = (int)args.NewItem.Source.CustomProperties["id"];
            playInfo = playList.FirstOrDefault(x => x.songid == songid);
            //显示试听信息
            if (playInfo.is_preview)
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Utils.ShowMessageToast("试听中,会员可以听完整哦", new List<MyUICommand> {
                        new MyUICommand(UserHelper.isLogin?"开通会员":"登录",async (toast,command)=>{
                            if (UserHelper.isLogin)
                            {
                                await Windows.System.Launcher.LaunchUriAsync(new Uri("https://account.bilibili.com/account/big"));
                            }
                            else
                            {
                                await Utils.ShowLoginDialog();
                            }
                        }),
                        new MyUICommand("关闭",(toast,command)=>{
                            (toast as MessageToast).Close();
                        }),
                    });
                });
            }
            

            foreach (var item in playList)
            {
                DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    item.select = false;
                    item.color = (SolidColorBrush)App.Current.Resources["COLOR_Foreground"];
                });
            }
            DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                playInfo.select = true;
                playInfo.color = (SolidColorBrush)App.Current.Resources["COLOR_Main"];
            });

            var _systemMediaTransportControls = mediaPlayer.SystemMediaTransportControls;
            SystemMediaTransportControlsDisplayUpdater updater = _systemMediaTransportControls.DisplayUpdater;
            updater.Type = MediaPlaybackType.Music;
            updater.MusicProperties.Artist = playInfo.author;
            updater.MusicProperties.Title = playInfo.title;
            updater.Thumbnail = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(new Uri(playInfo.pic));
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
        /// <summary>
        /// 添加播放
        /// </summary>
        /// <param name="play"></param>
        public void AddPlay(PlayModel play)
        {
            if (playList.FirstOrDefault(x => x.songid == play.songid) != null)
            {
                return;
            }
            if (!play.pic.Contains("300w_300h_1e_1c"))
            {
                play.pic = play.pic + "@300w_300h_1e_1c.jpg";
            }
            playList.Add(play);

            var binder = new MediaBinder();
            binder.Token = play.songid.ToString();
            binder.Binding += Binder_Binding;
            var source = MediaSource.CreateFromMediaBinder(binder);
            source.CustomProperties["id"] = play.songid;
            mediaPlaybackList.Items.Add(new MediaPlaybackItem(source));


            //var item = new MediaPlaybackItem(MediaSource.CreateFromUri(play.play_url));
            //mediaPlaybackList.Items.Add(item);


        }

        /// <summary>
        /// 替换播放列表
        /// </summary>
        /// <param name="play"></param>
        public void ReplacePlayList(IList<PlayModel> list)
        {
            ClearPlaylist();
            foreach (var item in list)
            {
                AddPlay(item);
            }
        }
        /// <summary>
        /// 清除播放列表
        /// </summary>
        public void ClearPlaylist()
        {
            playList.Clear();
            playInfo = new PlayModel()
            {
                title = "哔哩哔哩 (゜-゜)つロ 干杯~",
                pic = "ms-appx:///Assets/StoreLogo.scale-100.png"
            };
            mediaPlaybackList.Items.Clear();
            bufferingProgress = 0;
            this.totalDuration = 0;
            this.position = 0;
            this.durationStr = "00:00 / 00:00";
        }
        /// <summary>
        /// 删除播放项
        /// </summary>
        /// <param name="playModel"></param>
        public void DeletePlayitem(PlayModel playModel)
        {
            if (mediaPlaybackList.Items.Count == 1)
            {
                ClearPlaylist();
                return;
            }
            //if (playModel.select)
            //{
            //    mediaPlaybackList.MoveNext();
            //}

            var id = playModel.songid;
            var playitem = mediaPlaybackList.Items.FirstOrDefault(x => x.Source.CustomProperties["id"].Equals(id));
            playList.Remove(playModel);
            mediaPlaybackList.Items.Remove(playitem);

        }

        private async void Binder_Binding(MediaBinder sender, MediaBindingEventArgs args)
        {

            var deferral = args.GetDeferral();
            loading = true;
            var songid = Convert.ToInt32(args.MediaBinder.Token);
            var item = playList.FirstOrDefault(x => x.songid == songid);
            var url = await GetUrl(songid);
            if (url == null)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    Utils.ShowMessageToast("无法播放" + item.title + ",已从播放列表移除");
                    playList.Remove(item);
                });
                mediaPlaybackList.Items.Remove(mediaPlaybackList.Items.FirstOrDefault(x => x.Source == sender.Source));
                loading = false;
                return;
            }
            if (url.type==-1)
            {
                item.is_preview = true;
            }
            else {
                item.is_preview = false;
            }

            item.play_url = new Uri(url.cdns[0]);
            item.qualities = url.qualities;
            item.backup_url = url.cdns;
            var contentUri = item.play_url;
            args.SetUri(contentUri);
            loading = false;
            deferral.Complete();
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
                    title = data.data.title,
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
