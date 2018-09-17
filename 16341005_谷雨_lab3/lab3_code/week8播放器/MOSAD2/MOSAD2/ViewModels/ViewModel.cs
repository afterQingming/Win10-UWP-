using System;
using System.ComponentModel;
using System.Diagnostics;
using Windows.Media;
using Windows.Media.Playback;
namespace MOSAD2.ViewModels
{
    class ViewModel:INotifyPropertyChanged
    {
        private MediaPlayer mediaPlayer;
        private MediaTimelineController mediaTimelineController;
        public MediaTimelineController MediaTimelineController { get { return mediaTimelineController; } set { mediaTimelineController = value; NotifyPropertyChanged("MediaTimelineController"); } }
        public MediaPlayer MediaPlayer { get { return mediaPlayer; }set { mediaPlayer = value; NotifyPropertyChanged("MediaPlayer"); } }
        private bool stop = true;
        public bool Stop { get { return !stop; }set { stop = value;NotifyPropertyChanged("Stop"); } }
        private bool pause = false;
        public bool Pause { get { return !pause; } set { pause = value; NotifyPropertyChanged("Pause"); } }
        private bool running = false;
        public bool Running { get { return running; } set { running = value; NotifyPropertyChanged("Running"); } }
        public ViewModel()
        {
            MediaPlayer = new MediaPlayer();
            MediaTimelineController = new MediaTimelineController();
            MediaPlayer.TimelineController = mediaTimelineController;
            
        }
        public void BeRunning()
        {
            Stop = false;
            Pause = false;
            Running = true;
        }
        public void BePause()
        {
            Stop = false;
            Pause = true;
            Running = true;
        }
        public void BeStop()
        {
            Stop = true;
            Pause = false;
            Running = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
