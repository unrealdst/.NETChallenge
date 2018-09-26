using BucketRepository.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest
{
    [TestClass]
    public class BucketRepositoryTests
    {
        private BucketRepository.BucketRepository bucketRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            bucketRepository = new BucketRepository.BucketRepository();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            bucketRepository.Dispose();
        }

        [TestMethod]
        public void Create_CreateNewBucket()
        {
            BucketDataModel result = bucketRepository.Create();

            Assert.AreEqual(1, result.Id);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public void Create_OneBucketExistThenCreateSecondBucket()
        {
            bucketRepository.Create();

            BucketDataModel result = bucketRepository.Create();

            Assert.AreEqual(2, result.Id);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(0, result.Items.Count);
        }

        [TestMethod]
        public void Delete_WhenRemoveExistingBucketThenTrue()
        {
            bucketRepository.Create();

            var result = bucketRepository.Delete(1);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Delete_WhenRemoveNotExistingBucketThenFalse()
        {
            bucketRepository.Create();

            var result = bucketRepository.Delete(2);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Read_EmptyBucketReturnEmptyBucket()
        {
            IEnumerable<BucketDataModel> result = bucketRepository.Read();

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        public void Read_OneBucketReturnBucket()
        {
            bucketRepository.Create();

            IEnumerable<BucketDataModel> result = bucketRepository.Read();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.First().Items);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(1, result.First().Id);
            Assert.AreEqual(0, result.First().Items.Count);
        }

        [TestMethod]
        public void Read_TwoBucketReturnTwoBuckets()
        {
            bucketRepository.Create();
            bucketRepository.Create();

            IEnumerable<BucketDataModel> result = bucketRepository.Read();

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.First().Items);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(1, result.First().Id);
            Assert.AreEqual(0, result.First().Items.Count);
            Assert.AreEqual(2, result.Last().Id);
            Assert.AreEqual(0, result.Last().Items.Count);
        }

        [TestMethod]
        public void ReadSingle_TwoBucketReturnSecondBuckets()
        {
            bucketRepository.Create();
            bucketRepository.Create();

            BucketDataModel result = bucketRepository.Read(2);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(2, result.Id);
        }

        [TestMethod]
        public void ReadSingle_TwoBucketReturnFirstBuckets()
        {
            bucketRepository.Create();
            bucketRepository.Create();

            BucketDataModel result = bucketRepository.Read(1);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void Update_TwoBucketReturnSecondBuckets()
        {
            bucketRepository.Create();
            bucketRepository.Create();
            var parameter = new BucketDataModel() { Id = 2, Items = new List<ItemDataModel>(){ new ItemDataModel() { Id = 3, Name = "name", PriceForUnit = 23, Quantity = 1 } } };

            BucketDataModel result = bucketRepository.Update(2, parameter);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Items);
            Assert.AreEqual(2, result.Id);
            Assert.AreEqual(1, result.Items.Count);
            Assert.AreEqual(3, result.Items.First().Id);
            Assert.AreEqual("name", result.Items.First().Name);
            Assert.AreEqual(23, result.Items.First().PriceForUnit);
            Assert.AreEqual(1, result.Items.First().Quantity);
        }

    }
}
