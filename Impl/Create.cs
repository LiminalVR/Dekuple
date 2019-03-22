// (C) 2012-2019 Christian Schladetsch. See https://github.com/cschladetsch/Flow.

using Flow.Impl;

#pragma warning disable 1685

namespace Flow
{
    /// <summary>
    /// Boot-strapper for the flow library using default implementations
    /// </summary>
    public static class Create
    {
        public static IKernel Kernel()
        {
            return NewFactory<Factory>().Kernel;
        }

        public static IFactory NewFactory<TF>() where TF : class, IFactory, new()
        {
            var kernel = new Kernel();
            var factory = new TF();

            kernel.Factory = factory;
            kernel.Kernel = kernel;
            factory.Kernel = kernel;

            kernel.Root = new Node { Kernel = kernel, Name = "Root" };

            return factory;
        }
    }
}
