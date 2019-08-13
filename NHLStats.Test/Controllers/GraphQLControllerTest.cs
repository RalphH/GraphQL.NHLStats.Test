using GraphQL.Client;
using GraphQL.Common.Request;
using GraphQL.Common.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace NHLStats.Controllers.Test
{
    public class GraphQLControllerTest
    {
        // integration tests. running NHLStats.Api via 'dotnet run' 1st

        [Fact]
        public async void Post_ShouldReturnRandomData()
        {            
            GraphQLClient gqlClient = new GraphQLClient("http://localhost:56301/graphql");
            var gqlRequest = new GraphQLRequest
            {
                Query = @"query { randomPlayer { name } }"
            };
            GraphQLResponse gqlResponse = await gqlClient.PostAsync(gqlRequest);
            Assert.Contains("randomPlayer", gqlResponse.Data);
        }

        [Fact]
        public async void Post_ShouldReturnNHLStatsQueryPlayerofTypeObject()
        {
            GraphQLClient gqlClient = new GraphQLClient("http://localhost:56301/graphql");
            var gqlRequest = new GraphQLRequest
            {
                Query = @"{
                  __type(name:" + char.ConvertFromUtf32(34) + "NHLStatsQuery" + char.ConvertFromUtf32(34) + @") {
                    name
                    fields {
                      name      
                      type {
                        name
                        kind
                        ofType {
                          name
                          kind
                        }         
                      }
                    }
                  }
                }"
            };
            GraphQLResponse gqlResponse = await gqlClient.PostAsync(gqlRequest);            
            JToken playerstypeofkind = gqlResponse.Data.SelectToken("$.__type.fields[?(@.name=='players')].type.ofType.kind"); //$.data.__type.fields[?(@.name=="players")].type.ofType.kind
            //IEnumerable<JToken> fieldstypes = gqlResponse.Data.SelectTokens("$..fields[?(@.name=='players')].type");
            Assert.Equal("OBJECT", playerstypeofkind);
        }
    }
}
