using com.howmuchof.squirrgithuels.wp.Resources;

namespace com.howmuchof.squirrgithuels.wp
{
    /// <summary>
    /// Provides access to string resources.
    /// </summary>
    public class LocalizedStrings
    {
        private static AppResources _localizedResources = new AppResources();

        public AppResources LocalizedResources { get { return _localizedResources; } }
    }
}