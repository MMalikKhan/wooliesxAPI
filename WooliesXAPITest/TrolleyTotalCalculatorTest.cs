using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using WooliesXAPI.DataContracts;
using WooliesXAPI.Services;
using Xunit;

namespace WooliesXAPITest
{
    public class TrolleyTotalCalculatorTest
    {
        [Fact]
        
        public  void CheckTrolleyTotalCaulculationSimpleCaseSuccess()
        {   
            //Arrange
            var logger = Mock.Of<ILogger<TrolleyTotalCalculator>>();
            var trolleyTotalCalculator = new TrolleyTotalCalculator(logger);

            var requestObj =
                "{\"products\":[{\"name\":\"1\",\"price\":2.0},{\"name\":\"2\",\"price\":5.0}],\"specials\":[{\"quantities\":[{\"name\":\"1\",\"quantity\":3},{\"name\":\"2\",\"quantity\":0}],\"total\":5.0},{\"quantities\":[{\"name\":\"1\",\"quantity\":1},{\"name\":\"2\",\"quantity\":2}],\"total\":10.0}],\"quantities\":[{\"name\":\"1\",\"quantity\":3},{\"name\":\"2\",\"quantity\":2}]}";
            
            var request = JsonConvert.DeserializeObject<GetTrolleyTotalRequest>(requestObj);
            var expectedTrolleyTotal = 14;
            
            //Act
            var lowestTrolleyTotal = trolleyTotalCalculator.CalculateLowestTrolleyTotal(request);

            //Assert
            Assert.Equal(expectedTrolleyTotal, lowestTrolleyTotal);
            
        }
        [Fact]
        public void CheckTrolleyTotalCaulculationComplexCaseSuccess()
        {
            //Arrange
            var logger = Mock.Of<ILogger<TrolleyTotalCalculator>>();
            var trolleyTotalCalculator = new TrolleyTotalCalculator(logger);
            var requestObj =
                "{\"products\":[{\"name\":\"Product 0\",\"price\":6.87769804237303}],\"specials\":[],\"quantities\":[{\"name\":\"Product 0\",\"quantity\":9}]}";
            var request = JsonConvert.DeserializeObject<GetTrolleyTotalRequest>(requestObj);
            var expectedTrolleyTotal = 61.89928238135727M;

            //Act
            var lowestTrolleyTotal = trolleyTotalCalculator.CalculateLowestTrolleyTotal(request);

            //Assert
            Assert.Equal(expectedTrolleyTotal, lowestTrolleyTotal);

        }

        [Fact]
        public void CheckTrolleyTotalCaulculationAnotherEdgeCaseSuccess()
        {
            //Arrange
            var logger = Mock.Of<ILogger<TrolleyTotalCalculator>>();
            var trolleyTotalCalculator = new TrolleyTotalCalculator(logger);
            var requestObj =
                "{\"products\":[{\"name\":\"Product 0\",\"price\":13.2070998815853},{\"name\":\"Product 1\",\"price\":10.0083002727517}],\"specials\":[{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":0},{\"name\":\"Product 1\",\"quantity\":3}],\"total\":10.7875576502741},{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":4},{\"name\":\"Product 1\",\"quantity\":9}],\"total\":11.7869855672865},{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":5},{\"name\":\"Product 1\",\"quantity\":0}],\"total\":10.9380746082547},{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":9},{\"name\":\"Product 1\",\"quantity\":7}],\"total\":12.1611965060409},{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":4},{\"name\":\"Product 1\",\"quantity\":8}],\"total\":2.18133143629825},{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":3},{\"name\":\"Product 1\",\"quantity\":2}],\"total\":10.0869935360074},{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":0},{\"name\":\"Product 1\",\"quantity\":2}],\"total\":14.6007625936025}],\"quantities\":[{\"name\":\"Product 0\",\"quantity\":0},{\"name\":\"Product 1\",\"quantity\":6}]}";
            var request = JsonConvert.DeserializeObject<GetTrolleyTotalRequest>(requestObj);
            var expectedTrolleyTotal = 21.5751153005482M;

            //Act
            var lowestTrolleyTotal = trolleyTotalCalculator.CalculateLowestTrolleyTotal(request);

            //Assert
            Assert.Equal(expectedTrolleyTotal, lowestTrolleyTotal);

        }


        [Fact]
        public void CheckTrolleyTotalCaulculationWithApplyMultipleSpecialsSuccess()
        {
            //Arrange
            var logger = Mock.Of<ILogger<TrolleyTotalCalculator>>();
            var trolleyTotalCalculator = new TrolleyTotalCalculator(logger);
            var requestObj =
                "{\"products\":[{\"name\":\"Product 0\",\"price\":9.9649406596855},{\"name\":\"Product 1\",\"price\":9.59257845282209}],\"specials\":[{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":5},{\"name\":\"Product 1\",\"quantity\":1}],\"total\":16.6315690902189},{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":2},{\"name\":\"Product 1\",\"quantity\":4}],\"total\":0.826659624042661},{\"quantities\":[{\"name\":\"Product 0\",\"quantity\":9},{\"name\":\"Product 1\",\"quantity\":4}],\"total\":15.9378605992339}],\"quantities\":[{\"name\":\"Product 0\",\"quantity\":7},{\"name\":\"Product 1\",\"quantity\":6}]}";
            var request = JsonConvert.DeserializeObject<GetTrolleyTotalRequest>(requestObj);
            var expectedTrolleyTotal = 27.050807167083651M;

            //Act
            var lowestTrolleyTotal = trolleyTotalCalculator.CalculateLowestTrolleyTotal(request);

            //Assert
            Assert.Equal(expectedTrolleyTotal, lowestTrolleyTotal);

        }
        
    }
}
