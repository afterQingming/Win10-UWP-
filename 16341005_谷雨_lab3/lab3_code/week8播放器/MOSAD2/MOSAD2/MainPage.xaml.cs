using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MOSAD2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ViewModels.ViewModel ViewModel = new ViewModels.ViewModel();
        private bool video = false;
       

        private void StateChange(bool IfVideo)
        {
            if (IfVideo)
            {
                video = true;
                ImageBrush imageBrush = new ImageBrush();
                imageBrush.ImageSource = new BitmapImage(new Uri("ms-appx:///Assets/3.jpg"));
                _compositionCanvas.Background = imageBrush;
                border.CornerRadius = new CornerRadius(0);
                Show();
            }
            else
            {
                video = false;
                if (ElementCompositionPreview.GetElementChildVisual(_compositionCanvas) != null)
                    ElementCompositionPreview.GetElementChildVisual(_compositionCanvas).Dispose();
                border.CornerRadius = new CornerRadius(1000);
            }
        }
        public MainPage()
        {
            this.InitializeComponent();
            ViewModel.MediaTimelineController.PositionChanged += ViewModelPositionChange;
            ViewModel.MediaTimelineController.Ended += ViewModelEnded;
            SizeChanged += SizeChange;

        }

        private void SizeChange(object sender, SizeChangedEventArgs e)
        {
            float a = Math.Min((float)e.NewSize.Width, (float)e.NewSize.Height);
            border.Width = a;
            border.Height = a;
            _compositionCanvas.Width = a;
            _compositionCanvas.Height = a;
            resetSize();
        }

        private void resetSize()
        {
            if(ElementCompositionPreview.GetElementChildVisual(_compositionCanvas) != null&&video)
            {
                ElementCompositionPreview.GetElementChildVisual(_compositionCanvas).Dispose();
                MediaPlayer _mediaPlayer = ViewModel.MediaPlayer;
                _mediaPlayer.SetSurfaceSize(new Size(_compositionCanvas.Width, _compositionCanvas.Height));

                var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
                MediaPlayerSurface surface = _mediaPlayer.GetSurface(compositor);
                SpriteVisual spriteVisual = compositor.CreateSpriteVisual();
                spriteVisual.Size = new System.Numerics.Vector2((float)_compositionCanvas.Width, (float)_compositionCanvas.Height);

                CompositionBrush brush = compositor.CreateSurfaceBrush(surface.CompositionSurface);
                spriteVisual.Brush = brush;

                ContainerVisual container = compositor.CreateContainerVisual();
                container.Children.InsertAtTop(spriteVisual);
                ElementCompositionPreview.SetElementChildVisual(_compositionCanvas, container);
            }
            
        }
        private void Show()
        {
            
            MediaPlayer _mediaPlayer = ViewModel.MediaPlayer;
            _mediaPlayer.SetSurfaceSize(new Size(_compositionCanvas.ActualWidth, _compositionCanvas.ActualHeight));

            var compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            MediaPlayerSurface surface = _mediaPlayer.GetSurface(compositor);

            SpriteVisual spriteVisual = compositor.CreateSpriteVisual();
            spriteVisual.Size =new System.Numerics.Vector2((float)_compositionCanvas.ActualWidth, (float)_compositionCanvas.ActualHeight);

            CompositionBrush brush = compositor.CreateSurfaceBrush(surface.CompositionSurface);
            spriteVisual.Brush = brush;

            ContainerVisual container = compositor.CreateContainerVisual();
            container.Children.InsertAtTop(spriteVisual);
            ElementCompositionPreview.SetElementChildVisual(_compositionCanvas, container);
        }
        private async void ViewModelPositionChange(MediaTimelineController sender, object args)
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                try
                {
                    timeLine.Value = sender.Position.TotalSeconds;
                    if (timeLine.Value == timeLine.Maximum)
                    {
                        if(timeLine.Value!=0 && timeLine.Maximum!=0)
                        Pause();
                    }
                }

                catch (Exception)
                {
                    throw new Exception("111");
                }
                
            });
        }
        private void ViewModelEnded(MediaTimelineController sender, object args)
        {
            timeLine.Value = 0;
            Pause();
        }
        private async void MediaSource_OpenOperationCompleted(MediaSource sender, MediaSourceOpenOperationCompletedEventArgs args)
        {
            var _duration = sender.Duration.GetValueOrDefault();

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                timeLine.Minimum = 0;
                timeLine.Maximum = _duration.TotalSeconds;
                timeLine.StepFrequency = 1;
            });
        }


        private async void OpenAndLoadAsync()
        {
            var openPicker = new FileOpenPicker();

            openPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.VideosLibrary;
            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".mp3");
            openPicker.FileTypeFilter.Add(".wma");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                var mediaSource = MediaSource.CreateFromStorageFile(file);
                mediaSource.OpenOperationCompleted += MediaSource_OpenOperationCompleted;
                ViewModel.MediaPlayer.Source = mediaSource;
                if (file.FileType == ".mp3" || file.FileType == ".wma")
                {
                    //
                    StateChange(false);
                    using (StorageItemThumbnail thumbnail = await file.GetThumbnailAsync(ThumbnailMode.MusicView, 300))
                    {
                        if (thumbnail != null && thumbnail.Type == ThumbnailType.Image)
                        {
                            var bitmapImage = new BitmapImage();
                            bitmapImage.SetSource(thumbnail);
                            var imageBrush = new ImageBrush();
                            imageBrush.ImageSource = bitmapImage;
                            _compositionCanvas.Background = imageBrush;
                        }
                    }

                }
                else
                {
                    StateChange(true);
                }
                Begin();
            }

            
        }
        private void AddButton_Clicked(object sender, RoutedEventArgs e)
        {
            Stop();
            OpenAndLoadAsync();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.MediaTimelineController.State == MediaTimelineControllerState.Running)
            {
                Pause();
                PauseButton.Icon = new SymbolIcon(Symbol.Play);
                PauseButton.Label = "Play";
            }
            else
            {
                Resume();
                PauseButton.Icon = new SymbolIcon(Symbol.Pause);
                PauseButton.Label = "Pause";
            }
        }
        private void PlayButton_Clicked(object sender, RoutedEventArgs e)
        {
            Begin();
        }
        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Stop();
            
        }
        
        private void Volume_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ViewModel.MediaPlayer.Volume = (double)(sender as Slider).Value/100;
        }
        
        private void FullScreenButton_Clicked(object sender, RoutedEventArgs e)
        {
            ApplicationView view = ApplicationView.GetForCurrentView();
            bool isInFullScreenMode = view.IsFullScreenMode;
            if (isInFullScreenMode)
            {
                view.ExitFullScreenMode();
            }
            else
            {
                view.TryEnterFullScreenMode();
            }
        }
        private void Resume()
        {
            ViewModel.MediaTimelineController.Resume();
            RotateTransformStoryboard.Resume();
            ViewModel.BeRunning();
        }
        private void Pause()
        {
            ViewModel.MediaTimelineController.Pause();
            RotateTransformStoryboard.Pause();

            ViewModel.BePause();
        }
        private void Begin()
        {
            if (ViewModel.MediaPlayer.Source != null){
                ViewModel.MediaTimelineController.Start();
                if (!video)
                {
                    RotateTransformStoryboard.Begin();
                }
                ViewModel.BeRunning();
            }
        }
        private void Stop()
        {
            
            ViewModel.MediaTimelineController.Pause();
            ViewModel.MediaPlayer.Source=null;
            StateChange(false);
            timeLine.Value = 0;
            timeLine.Minimum = 0;
            timeLine.Maximum = 0;
            RotateTransformStoryboard.Stop();
            ViewModel.BeStop();
            PauseButton.Label = "Play";
        }

        private void timeLine_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            ViewModel.MediaTimelineController.Position = TimeSpan.FromSeconds(timeLine.Value);
        }
    }
    
}
