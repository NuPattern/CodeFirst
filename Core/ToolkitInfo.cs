namespace NuPattern
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;

    public class ToolkitInfo : IToolkitInfo
    {
        private JObject info;

        public ToolkitInfo(JObject info)
        {
            this.info = info;
        }

        public string Id 
        {
            get { return info.Get(() => Id); }
            set { info.Set(() => Id, value); }
        }

        public string Version
        {
            get { return info.Get(() => Version); }
            set { info.Set(() => Version, value); }
        }
    }
}