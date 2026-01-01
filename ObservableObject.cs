using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AttendenceManagementSystem
{
    public class ObservableObject : INotifyPropertyChanged
    {
        // Nullable event for property change notification
        public event PropertyChangedEventHandler? PropertyChanged;

        // Method to notify property changes
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helper method to simplify property setting and notification
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
