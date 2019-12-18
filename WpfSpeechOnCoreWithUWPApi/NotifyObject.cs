using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfSpeechOnCoreWithUWPApi
{
    public abstract class NotifyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetProperty<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(property, value))
            {
                property = value;
                RaisePropertyChanged(propertyName);
            }
        }

        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
