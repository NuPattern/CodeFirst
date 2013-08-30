namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    public class ToolkitInfo : IToolkitInfo
    {
        private JObject info;
        private SemanticVersion version;

        public ToolkitInfo(JObject info)
        {
            Guard.NotNull(() => info, info);

            this.info = info;
        }

        public string Id 
        {
            get { return info.Get(() => Id); }
            set { info.Set(() => Id, value); }
        }

        public SemanticVersion Version
        {
            get 
            {
                if (version != null)
                    return version;

                var value = info.Get<string>("Version");
                if (string.IsNullOrEmpty(value) || !SemanticVersion.TryParse(value, out version))
                    return null;

                return version;
            }
            set 
            {
                Guard.NotNull(() => Version, value);
                version = value;
                info.Set("Version", version.ToString());
            }
        }
    }
}