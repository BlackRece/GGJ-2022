namespace GGJ2022 {
    public static class IoC {
        private static IDependencyResolver _resolver;

        public static void Initialise(IDependencyResolver resolver) =>
            _resolver = resolver;

        public static void Register<T>(object obj) =>
            _resolver.Register<T>(obj);

        public static T Resolve<T>() =>
            _resolver.Resolve<T>();

        public static void Dispose<T>(object obj) =>
            _resolver.Dispose<T>(obj);
    }
}