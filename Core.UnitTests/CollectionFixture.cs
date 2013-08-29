namespace NuPattern
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using System;
    using System.Linq;
    using Xunit;

    public class CollectionFixture
    {
        [Fact]
        public void when_json_contains_items_then_exposes_them_as_components()
        {
            var json = (JObject)JsonConvert.DeserializeObject(@"
                {
                    '$schema': 'ICollection',
                    '$items': [
                        { '$schema': 'IElement' }, 
                        { '$schema': 'IElement' } 
                    ]
                }");

            var collection = new Collection(json);

            Assert.Equal(2, collection.Items.Count());
        }
    }
}