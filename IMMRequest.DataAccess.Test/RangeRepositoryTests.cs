using IMMRequest.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Range = IMMRequest.Domain.Range;

namespace IMMRequest.DataAccess.Tests
{
    [ExcludeFromCodeCoverage]
    [TestClass]
    public class RangeRepositoryTests
    {
        private static AdditionalField additionalField = new AdditionalField()
        {
            Id = Guid.NewGuid(),
            FieldType = FieldType.Texto,
            Range = new List<Range>(),
            Name = "Tipo de taxi",
            Type = new Domain.Type()
            {
                Id = Guid.NewGuid()
            }
        };

        Range oneRangeAddField = new Range()
        {
            Id = Guid.NewGuid(),
            AdditionalField = additionalField,
            Value = "Fono taxi"
        };

        Range anotherRangeAddField = new Range()
        {
            Id = Guid.NewGuid(),
            AdditionalField = additionalField,
            Value = "Taxi aeropuerto"
        };

        private RangeRepository rangeRepositoryInMemory = new RangeRepository(
            ContextFactory.GetNewContext(ContextType.MEMORY));

        [TestInitialize]
        public void Initialize()
        {
            rangeRepositoryInMemory.Empty();
        }

        [TestMethod]
        public void GetRange_InexistentRange_ShouldReturnNull()
        {
            Range rangeById = rangeRepositoryInMemory.Get(oneRangeAddField.Id);
            Assert.IsNull(rangeById);
        }

        [TestMethod]
        public void GetRange_ExistentRange_ShouldReturnSpecificRangeFromDB()
        {
            rangeRepositoryInMemory.Add(oneRangeAddField);
            rangeRepositoryInMemory.Add(anotherRangeAddField);
            rangeRepositoryInMemory.SaveChanges();

            Assert.AreEqual(rangeRepositoryInMemory.Get(oneRangeAddField.Id),
                oneRangeAddField);
        }
    }
}
