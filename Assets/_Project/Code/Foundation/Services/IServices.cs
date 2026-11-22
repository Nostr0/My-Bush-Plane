namespace MaintGames.Bush.Foundation.Services
{
    /// <summary>
    /// Read access to the services created by the composition root. Only the composition root and the
    /// <c>Initialize(...)</c> methods it calls may hold an <see cref="IServices"/>. Resolve once, cache the result.
    /// </summary>
    public interface IServices
    {
        /// <summary>Returns the service registered as <typeparamref name="T"/>. Throws if it is missing.</summary>
        T Get<T>() where T : class;

        /// <summary>Returns <c>false</c> instead of throwing if <typeparamref name="T"/> is not registered.</summary>
        bool TryGet<T>(out T service) where T : class;
    }
}
