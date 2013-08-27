namespace NuPattern.JsonMigrations
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using Xunit;

    public class JsonMigrations
    {
        [Fact]
        public void when_deserializing_to_v2_then_can_provide_default_value()
        {
            var app1 = new AppV1 { Id = 23 };

            var settings = new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                //TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Objects,
                //TypeNameAssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full,
                //ConstructorHandling = ConstructorHandling.Default,
                //ReferenceLoopHandling = ReferenceLoopHandling.Ignore, 
                //PreserveReferencesHandling = PreserveReferencesHandling.Objects, 
                //NullValueHandling = NullValueHandling.Ignore, 
                //MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(app1, Formatting.Indented);

            //Console.WriteLine(json);
            
            var app2 = JsonConvert.DeserializeObject<AppV2>(json, settings);

            Assert.Equal(app1.Id, app2.Id);
            Assert.Equal("App", app2.Title);
        }
    }

    public class AppV1
    {
        public int Id { get; set; }
    }

    public class AppV2
    {
        //public AppV2(int id)
        //{
        //    this.Id = id;
        //}

        public int Id { get; set; }

        [DefaultValue("App")]
        public string Title { get; set; }
    }
}