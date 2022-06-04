namespace CockroachHunter.Utils
{
    public class BindingValue<T>
    where T: struct
    {
        public delegate void OnValueChangeEvent(T value, T oldValue);
        public event OnValueChangeEvent OnChange;

        public T Value
        {
            get => _value;
            set
            {
                OnChange?.Invoke(value,_value);
                _value = value;
            }
        }

        private T _value;
    }
}