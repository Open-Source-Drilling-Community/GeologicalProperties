using System.Net.Http.Headers;
using GeologicalPropertiesApp.GeologicalProperties.ModelShared;

namespace ServiceTest
{
    public class Tests
    {
        // testing outside Visual Studio requires using http port (https faces authentication issues both in console and on github)
        private static string host = "http://localhost:8080/";
        //private static string host = "https://localhost:5001/";
        //private static string host = "https://localhost:44368/";
        //private static string host = "http://localhost:54949/";
        private static HttpClient httpClient;
        private static Client nSwagClient;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }; // temporary workaround for testing purposes: bypass certificate validation (not recommended for production environments due to security risks)
            httpClient = new HttpClient(handler);
            httpClient.BaseAddress = new Uri(host + "GeologicalProperties/api/");
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            nSwagClient = new Client(httpClient.BaseAddress.ToString(), httpClient);
        }
        public GeologicalProperties ConstructGeologicalProperties(MetaInfo metaInfo, GeologicalPropertyEntry geologicalPropertyEntryRow1, GeologicalPropertyEntry geologicalPropertyEntryRow2)
        {
            DateTimeOffset creationDate = DateTimeOffset.UtcNow;                     
            return new GeologicalProperties()
            {
                MetaInfo = metaInfo,
                Name = "My test GeologicalProperties",
                Description = "My test GeologicalProperties",
                CreationDate = creationDate,
                LastModificationDate = creationDate,
                GeologicalPropertyTable = [geologicalPropertyEntryRow1, geologicalPropertyEntryRow2]
            };
        }
        public GaussianDrillingProperty ConstructGaussianDrillingProperty(double? val)
        {                        
            return new GaussianDrillingProperty
            {
                GaussianValue = new GaussianDistribution
                {
                    Mean = val
                }
            };
        }
        public GeologicalPropertyEntry ConstructGeologicalPropertiesTable()
        {
            return new GeologicalPropertyEntry()
            {
                InternalFrictionAngle = ConstructGaussianDrillingProperty(null),
                UnconfinedCompressiveStrength = ConstructGaussianDrillingProperty(null),
                ConfinedCompressiveStrength = ConstructGaussianDrillingProperty(null),
                Porosity = ConstructGaussianDrillingProperty(null),
                Permeability = ConstructGaussianDrillingProperty(null),
                MeasuredDepth = ConstructGaussianDrillingProperty(null),
                PressureDifferential = ConstructGaussianDrillingProperty(null),
                DataType = GeologicalPropertyTableOrigin.Measured
            };
        }

        [Test]
        public async Task Test_GeologicalProperties_GET()
        {
            #region post a GeologicalProperties
            Guid guid = Guid.NewGuid();
            MetaInfo metaInfo = new() { ID = guid };
            // Create 1st instance of permeability
            
            GeologicalPropertyEntry geologicalPropertyEntryRow1 = ConstructGeologicalPropertiesTable();
            // Create 2nd instance of permeability           
            
            GeologicalPropertyEntry geologicalPropertyEntryRow2 = ConstructGeologicalPropertiesTable();
            GeologicalProperties geologicalProperties = ConstructGeologicalProperties(metaInfo, geologicalPropertyEntryRow1, geologicalPropertyEntryRow2);

            try
            {
                await nSwagClient.PostGeologicalPropertiesAsync(geologicalProperties);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST given GeologicalProperties\n" + ex.Message);
            }
            #endregion

            #region GetAllGeologicalPropertiesId
            List<Guid> idList = [];
            try
            {
                idList = (List<Guid>)await nSwagClient.GetAllGeologicalPropertiesIdAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET all GeologicalProperties ids\n" + ex.Message);
            }
            Assert.That(idList, Is.Not.Null);
            Assert.That(idList, Does.Contain(guid));
            #endregion

            #region GetAllGeologicalPropertiesMetaInfo
            List<MetaInfo> metaInfoList = [];
            try
            {
                metaInfoList = (List<MetaInfo>)await nSwagClient.GetAllGeologicalPropertiesMetaInfoAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET all GeologicalProperties metainfos\n" + ex.Message);
            }
            Assert.That(metaInfoList, Is.Not.Null);
            IEnumerable<MetaInfo> metaInfoList2 =
                from elt in metaInfoList
                where elt.ID == guid
                select elt;
            Assert.That(metaInfoList2, Is.Not.Null);
            Assert.That(metaInfoList2, Is.Not.Empty);
            #endregion

            #region GetAllGeologicalPropertiesById
            GeologicalProperties? geologicalProperties2 = null;
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Not.Null);
            Assert.That(geologicalProperties2.Name, Is.EqualTo(geologicalProperties.Name));
            #endregion

            #region GetAllGeologicalPropertiesLight
            List<GeologicalPropertiesLight> geologicalPropertiesLightList = [];
            try
            {
                geologicalPropertiesLightList = (List<GeologicalPropertiesLight>)await nSwagClient.GetAllGeologicalPropertiesLightAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the list of GeologicalPropertiesLight\n" + ex.Message);
            }
            Assert.That(geologicalPropertiesLightList, Is.Not.Null);
            Assert.That(geologicalPropertiesLightList, Is.Not.Empty);
            IEnumerable<GeologicalPropertiesLight> geologicalPropertiesLightList2 =
                from elt in geologicalPropertiesLightList
                where elt.Name == geologicalProperties.Name
                select elt;
            Assert.That(geologicalPropertiesLightList2, Is.Not.Null);
            Assert.That(geologicalPropertiesLightList2, Is.Not.Empty);
            #endregion

            #region GetAllGeologicalProperties
            List<GeologicalProperties> geologicalPropertiesList = new();
            try
            {
                geologicalPropertiesList = (List<GeologicalProperties>)await nSwagClient.GetAllGeologicalPropertiesAsync();
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the list of GeologicalProperties\n" + ex.Message);
            }
            Assert.That(geologicalPropertiesList, Is.Not.Null);
            IEnumerable<GeologicalProperties> geologicalPropertiesList2 =
                from elt in geologicalPropertiesList
                where elt.Name == geologicalProperties.Name
                select elt;
            Assert.That(geologicalPropertiesList2, Is.Not.Null);
            Assert.That(geologicalPropertiesList2, Is.Not.Empty);
            #endregion

            #region finally delete the new ID
            geologicalProperties2 = null;
            try
            {
                await nSwagClient.DeleteGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE GeologicalProperties of given Id\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_GeologicalProperties_POST()
        {
            #region trying to post an empty guid
            Guid guid = Guid.Empty;
            MetaInfo metaInfo = new() { ID = guid };
            // Create 1st instance of permeability            
            
            GeologicalPropertyEntry geologicalPropertyEntryRow1 = ConstructGeologicalPropertiesTable();
            
            // Create 2nd instance of permeability            
            GeologicalPropertyEntry geologicalPropertyEntryRow2 = ConstructGeologicalPropertiesTable();
            GeologicalProperties geologicalProperties = ConstructGeologicalProperties(metaInfo, geologicalPropertyEntryRow1, geologicalPropertyEntryRow2);
            GeologicalProperties? geologicalProperties2 = null;
            try
            {
                await nSwagClient.PostGeologicalPropertiesAsync(geologicalProperties);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(400));
                TestContext.WriteLine("Impossible to POST GeologicalProperties with empty Guid\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(Guid.Empty);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(400));
                TestContext.WriteLine("Impossible to GET GeologicalProperties identified by an empty Guid\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Null);
            #endregion

            #region post some corrupted data
            // post data with missing input that fails the calculation process
            #endregion

            #region posting a new ID in a valid state
            guid = Guid.NewGuid();
            metaInfo = new() { ID = guid };
            geologicalProperties.MetaInfo = metaInfo;
            try
            {
                await nSwagClient.PostGeologicalPropertiesAsync(geologicalProperties);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST GeologicalProperties although it is in a valid state\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Not.Null);
            Assert.That(geologicalProperties2.MetaInfo, Is.Not.Null);
            Assert.That(geologicalProperties2.MetaInfo.ID, Is.EqualTo(guid));
            Assert.That(geologicalProperties2.Name, Is.EqualTo(geologicalProperties.Name));
            #endregion

            #region trying to repost the same ID
            bool conflict = false;
            try
            {
                await nSwagClient.PostGeologicalPropertiesAsync(geologicalProperties);
            }
            catch (ApiException ex)
            {
                conflict = true;
                Assert.That(ex.StatusCode, Is.EqualTo(409));
                TestContext.WriteLine("Impossible to POST existing GeologicalProperties\n" + ex.Message);
            }
            Assert.That(conflict, Is.True);
            #endregion

            #region finally delete the new ID
            geologicalProperties2 = null;
            try
            {
                await nSwagClient.DeleteGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE GeologicalProperties of given Id\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_GeologicalProperties_PUT()
        {
            #region posting a new ID
            Guid guid = Guid.NewGuid();
            MetaInfo metaInfo = new() { ID = guid };
            // Create 1st instance of permeability            
            
            GeologicalPropertyEntry geologicalPropertyEntryRow1 = ConstructGeologicalPropertiesTable();
            // Create 2nd instance of permeability            
            
            GeologicalPropertyEntry geologicalPropertyEntryRow2 = ConstructGeologicalPropertiesTable();
            GeologicalProperties geologicalProperties = ConstructGeologicalProperties(metaInfo, geologicalPropertyEntryRow1, geologicalPropertyEntryRow2);
            GeologicalProperties? geologicalProperties2 = null;
            try
            {
                await nSwagClient.PostGeologicalPropertiesAsync(geologicalProperties);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST GeologicalProperties\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Not.Null);
            Assert.That(geologicalProperties2.MetaInfo, Is.Not.Null);
            Assert.That(geologicalProperties2.MetaInfo.ID, Is.EqualTo(guid));
            Assert.That(geologicalProperties2.Name, Is.EqualTo(geologicalProperties.Name));
            #endregion

            #region updating the new Id
            geologicalProperties.Name = "My test GeologicalProperties with modified name";
            geologicalProperties.LastModificationDate = DateTimeOffset.UtcNow;
            try
            {
                await nSwagClient.PutGeologicalPropertiesByIdAsync(geologicalProperties.MetaInfo.ID, geologicalProperties);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to PUT GeologicalProperties of given Id\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the updated GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Not.Null);
            Assert.That(geologicalProperties2.MetaInfo, Is.Not.Null);
            Assert.That(geologicalProperties2.MetaInfo.ID, Is.EqualTo(geologicalProperties.MetaInfo.ID));
            Assert.That(geologicalProperties2.Name, Is.EqualTo(geologicalProperties.Name));
            #endregion

            #region finally delete the new ID
            geologicalProperties2 = null;
            try
            {
                await nSwagClient.DeleteGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE GeologicalProperties of given Id\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Null);
            #endregion
        }

        [Test]
        public async Task Test_GeologicalProperties_DELETE()
        {
            #region posting a new ID
            Guid guid = Guid.NewGuid();
            MetaInfo metaInfo = new() { ID = guid };
            DateTimeOffset creationDate = DateTimeOffset.UtcNow;
            // Create 1st instance of permeability            
            
            GeologicalPropertyEntry geologicalPropertyEntryRow1 = ConstructGeologicalPropertiesTable();
            // Create 2nd instance of permeability            
            
            GeologicalPropertyEntry geologicalPropertyEntryRow2 = ConstructGeologicalPropertiesTable();
            GeologicalProperties geologicalProperties = ConstructGeologicalProperties(metaInfo, geologicalPropertyEntryRow1, geologicalPropertyEntryRow2);
            GeologicalProperties? geologicalProperties2 = null;
            try
            {
                await nSwagClient.PostGeologicalPropertiesAsync(geologicalProperties);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to POST GeologicalProperties\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to GET the GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Not.Null);
            Assert.That(geologicalProperties2.MetaInfo, Is.Not.Null);
            Assert.That(geologicalProperties2.MetaInfo.ID, Is.EqualTo(geologicalProperties.MetaInfo.ID));
            Assert.That(geologicalProperties2.Name, Is.EqualTo(geologicalProperties.Name));
            #endregion

            #region finally delete the new ID
            geologicalProperties2 = null;
            try
            {
                await nSwagClient.DeleteGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                TestContext.WriteLine("Impossible to DELETE GeologicalProperties of given Id\n" + ex.Message);
            }
            try
            {
                geologicalProperties2 = await nSwagClient.GetGeologicalPropertiesByIdAsync(guid);
            }
            catch (ApiException ex)
            {
                Assert.That(ex.StatusCode, Is.EqualTo(404));
                TestContext.WriteLine("Impossible to GET deleted GeologicalProperties of given Id\n" + ex.Message);
            }
            Assert.That(geologicalProperties2, Is.Null);
            #endregion
        }

        

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            httpClient?.Dispose();
        }
    }
}