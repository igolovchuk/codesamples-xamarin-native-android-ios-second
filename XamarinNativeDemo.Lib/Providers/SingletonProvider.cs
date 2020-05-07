using System;
namespace XamarinNativeDemo.Providers
{
    public abstract class SingletonProvider<T>
        where T : SingletonProvider<T>, new()
    {
        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        private static Lazy<T> _instanse = new Lazy<T>(() => new T());

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static T Instance => _instanse.Value;
    }
}
