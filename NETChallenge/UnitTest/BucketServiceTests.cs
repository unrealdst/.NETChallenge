using BucketRepository;
using BucketRepository.Models;
using BucketService;
using BucketService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class BucketServiceTests
    {
        private BucketService.BucketService bucketService;
        private Mock<IBucketRepository> bucketRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            bucketRepository = new Mock<IBucketRepository>();
            bucketService = new BucketService.BucketService(bucketRepository.Object);
        }

        [TestMethod]
        public void Add_WhenNullParamterThenException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => { bucketService.Add(null); });
        }

        [TestMethod]
        public void Add_WhenBucketNullThenException()
        {
            ItemDomainModel parameter = new ItemDomainModel();
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns((BucketDataModel)null);

            Assert.ThrowsException<Exception>(() => { bucketService.Add(parameter); });
        }

        [TestMethod]
        public void Add_WhenItemIsCorrectThenAddNewItem()
        {
            ItemDomainModel parameter = new ItemDomainModel() { Name = "name", PriceForUnit = 23, Quantity = 1 };
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(new BucketDataModel() { Items = new System.Collections.Generic.List<ItemDataModel>() });

            bucketService.Add(parameter);

            bucketRepository.Verify(x => x.Update(1, It.Is<BucketDataModel>(y => y.Id == 0
                                                                           && y.Items.Count == 1
                                                                           && y.Items.First().Id == 1
                                                                           && y.Items.First().Name == "name"
                                                                           && y.Items.First().PriceForUnit == 23
                                                                           && y.Items.First().Quantity == 1)), Times.Once);
        }

        [TestMethod]
        public void Get_WhenBucketNullThenException()
        {
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns((BucketDataModel)null);

            Assert.ThrowsException<Exception>(() => { bucketService.Get(); });
        }

        [TestMethod]
        public void Get_WhenBucketExistThenReturnBucket()
        {
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(new BucketDataModel() { Items = new List<ItemDataModel>() { new ItemDataModel() { Name = "name", PriceForUnit = 23, Quantity = 1 } } });

            var result = bucketService.Get();

            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual("name", result.Items.First().Name);
            Assert.AreEqual(23, result.Items.First().PriceForUnit);
            Assert.AreEqual(1, result.Items.First().Quantity);
            Assert.IsInstanceOfType(result, typeof(BucketDomainModel));
        }

        [TestMethod]
        public void Quantity_WhenBucketNullThenException()
        {
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns((BucketDataModel)null);

            Assert.ThrowsException<Exception>(() => { bucketService.Quantity(1, 1); });
        }

        [TestMethod]
        public void Quantity_WhenOneItemOnListChangeQuantityThenUpdate()
        {
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(new BucketDataModel() { Items = new List<ItemDataModel>() { new ItemDataModel() { Id = 1, Name = "name", PriceForUnit = 23, Quantity = 1 } } });

            bucketService.Quantity(1, 2);

            bucketRepository.Verify(x => x.Update(1, It.Is<BucketDataModel>(y => y.Id == 0
                                                               && y.Items.Count == 1
                                                               && y.Items.First().Id == 1
                                                               && y.Items.First().Name == "name"
                                                               && y.Items.First().PriceForUnit == 23
                                                               && y.Items.First().Quantity == 2)), Times.Once);
        }

        [TestMethod]
        public void Quantity_WhenTwoItemOnListChangeQuantityThenUpdate()
        {
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(new BucketDataModel()
            {
                Items = new List<ItemDataModel>() { new ItemDataModel() { Id = 1, Name = "name", PriceForUnit = 23, Quantity = 1 },
                                                                                                                                     new ItemDataModel() { Id = 2, Name = "name2", PriceForUnit = 23, Quantity = 3 }}
            });

            bucketService.Quantity(1, 2);

            bucketRepository.Verify(x => x.Update(1, It.Is<BucketDataModel>(y => y.Id == 0
                                                               && y.Items.Count == 2
                                                               && y.Items.First().Id == 1
                                                               && y.Items.First().Name == "name"
                                                               && y.Items.First().PriceForUnit == 23
                                                               && y.Items.First().Quantity == 2
                                                               && y.Items[1].Id == 2
                                                               && y.Items[1].Name == "name2"
                                                               && y.Items[1].PriceForUnit == 23
                                                               && y.Items[1].Quantity == 3)), Times.Once);
        }

        [TestMethod]
        public void Quantity_WhenChangeQuanityToZeroThenRemoveItemFromList()
        {
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(new BucketDataModel()
            {
                Items = new List<ItemDataModel>() { new ItemDataModel() { Id = 1, Name = "name", PriceForUnit = 23, Quantity = 1 },
                                                                                                                                     new ItemDataModel() { Id = 2, Name = "name2", PriceForUnit = 23, Quantity = 3 }}
            });

            bucketService.Quantity(1, 0);

            bucketRepository.Verify(x => x.Update(1, It.Is<BucketDataModel>(y => y.Id == 0
                                                               && y.Items.Count == 1
                                                               && y.Items.First().Id == 2
                                                               && y.Items.First().Name == "name2"
                                                               && y.Items.First().PriceForUnit == 23
                                                               && y.Items.First().Quantity == 3)), Times.Once);
        }

        [TestMethod]
        public void Quantity_WhenRemovingFromSingleElementListThenEmptyList()
        {
            bucketRepository.Setup(x => x.Read(It.IsAny<int>())).Returns(new BucketDataModel() { Items = new List<ItemDataModel>() { new ItemDataModel() { Id = 1, Name = "name", PriceForUnit = 23, Quantity = 1 } } });

            bucketService.Quantity(1, 0);

            bucketRepository.Verify(x => x.Update(1, It.Is<BucketDataModel>(y => y.Id == 0
                                                               && y.Items.Count == 0)), Times.Once);
        }

    }
}
