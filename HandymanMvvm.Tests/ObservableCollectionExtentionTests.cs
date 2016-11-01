using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using HandymanMvvm.Collections;
using HandymanMvvm.ViewModels;

namespace HandymanMvvm.Tests
{
    [TestClass]
    public class ObservableCollectionExtentionTests
    {
        [TestMethod]
        public void WhenOnlyUpdatesExist_ShouldOnlyUpdate()
        {
            var freshList = new List<ClassA>
            {
                new ClassA { FieldA1 = "1", FieldA2 = "A" },
                new ClassA { FieldA1 = "2", FieldA2 = "B" },
                new ClassA { FieldA1 = "3", FieldA2 = "C" },
            };
            var existingList = new ObservableCollection<ClassB>
            {
                new ClassB { FieldB1 = "1", FieldB2 = "X" },
                new ClassB { FieldB1 = "2", FieldB2 = "Y" },
                new ClassB { FieldB1 = "3", FieldB2 = "Z" },
            };

            //update
            existingList.Refresh(freshList,
                matchCondition: ((from, to) => from.FieldA1 == to.FieldB1),
                createAndUpdateAction: ((from, to) =>
                {
                    to.FieldB2 = from.FieldA2;
                }));

            //test result
            Assert.IsTrue(existingList[0].FieldB2 == "A" &&
                          existingList[1].FieldB2 == "B" &&
                          existingList[2].FieldB2 == "C" &&
                          existingList.Count == 3);
        }

        [TestMethod]
        public void WhenAddAndUpdatesExist_ShouldOnlyAddAndUpdate()
        {
            var freshList = new List<ClassA>
            {
                new ClassA { FieldA1 = "1", FieldA2 = "A" },
                new ClassA { FieldA1 = "2", FieldA2 = "B" },
                new ClassA { FieldA1 = "3", FieldA2 = "C" },
            };
            var existingList = new ObservableCollection<ClassB>
            {
                new ClassB { FieldB1 = "1", FieldB2 = "X" },
                new ClassB { FieldB1 = "2", FieldB2 = "Y" },
            };

            //update
            existingList.Refresh(freshList,
                matchCondition: ((from, to) => from.FieldA1 == to.FieldB1),
                createAndUpdateAction: ((from, to) =>
                {
                    to.FieldB1 = from.FieldA1;
                    to.FieldB2 = from.FieldA2;
                }));

            //test result
            Assert.IsTrue(existingList[0].FieldB2 == "A" &&
                          existingList[1].FieldB2 == "B" &&
                          existingList[2].FieldB2 == "C" &&
                          existingList.Count == 3);
        }

        [TestMethod]
        public void WhenDeleteAndUpdatesExist_ShouldOnlyDeleteAndUpdate()
        {
            var freshList = new List<ClassA>
            {
                new ClassA { FieldA1 = "1", FieldA2 = "A" },
                new ClassA { FieldA1 = "2", FieldA2 = "B" },
            };
            var existingList = new ObservableCollection<ClassB>
            {
                new ClassB { FieldB1 = "1", FieldB2 = "X" },
                new ClassB { FieldB1 = "2", FieldB2 = "Y" },
                new ClassB { FieldB1 = "3", FieldB2 = "Z" },
            };

            //update
            existingList.Refresh(freshList,
                matchCondition: ((from, to) => from.FieldA1 == to.FieldB1),
                createAndUpdateAction: ((from, to) =>
                {
                    to.FieldB2 = from.FieldA2;
                }));

            Assert.IsTrue(existingList[0].FieldB2 == "A" &&
                          existingList[1].FieldB2 == "B" &&
                          existingList.Count == 2);
        }

        [TestMethod]
        public void WhenAddDeleteAndUpdatesExist_ShouldOnlyAddDeleteAndUpdate()
        {
            var freshList = new List<ClassA>
            {
                new ClassA { FieldA1 = "2", FieldA2 = "B" },
                new ClassA { FieldA1 = "3", FieldA2 = "C" },
            };
            var existingList = new ObservableCollection<ClassB>
            {
                new ClassB { FieldB1 = "1", FieldB2 = "X" },
                new ClassB { FieldB1 = "2", FieldB2 = "Y" },
            };

            //update
            existingList.Refresh(freshList,
                matchCondition: ((from, to) => from.FieldA1 == to.FieldB1),
                createAndUpdateAction: ((from, to) =>
                {
                    to.FieldB2 = from.FieldA2;
                }));

            Assert.IsTrue(existingList[0].FieldB2 == "B" &&
                          existingList[1].FieldB2 == "C" &&
                          existingList.Count == 2);
        }

        [TestMethod]
        public void WhenAddAndDeleteExist_ShouldOnlyAddAndDelete()
        {
            var freshList = new List<ClassA>
            {
                new ClassA { FieldA1 = "1", FieldA2 = "A" },
            };
            var existingList = new ObservableCollection<ClassB>
            {
                new ClassB { FieldB1 = "2", FieldB2 = "X" },
            };

            //update
            existingList.Refresh(freshList,
                matchCondition: ((from, to) => from.FieldA1 == to.FieldB1),
                createAndUpdateAction: ((from, to) =>
                {
                    to.FieldB2 = from.FieldA2;
                }));

            Assert.IsTrue(existingList[0].FieldB2 == "A" && existingList.Count == 1);
        }

        [TestMethod]
        public void WhenNewSorting_ShouldSortCorrectly_SenarioA()
        {
            var freshList = new List<ClassA>
            {
                new ClassA { FieldA1 = "1", FieldA2 = "A" },
                new ClassA { FieldA1 = "4", FieldA2 = "D" },
                new ClassA { FieldA1 = "2", FieldA2 = "B" },
                new ClassA { FieldA1 = "3", FieldA2 = "C" },
                new ClassA { FieldA1 = "5", FieldA2 = "E" },
            };
            var existingList = new ObservableCollection<ClassBRefreshable>
            {
                new ClassBRefreshable { FieldB1 = "4", FieldB2 = "C" },
                new ClassBRefreshable { FieldB1 = "2", FieldB2 = "B" },
            };

            //update
            existingList.RefreshModels(freshList,
                matchCondition: ((from, to) => from.FieldA1 == to.FieldB1),
                orderBy: (to) => to.FieldB1);

            Assert.IsTrue(existingList[0].FieldB1 == "1" &&
                          existingList[1].FieldB1 == "2" && existingList[1].FieldB2 == "B" &&
                          existingList[2].FieldB1 == "3" &&
                          existingList[3].FieldB1 == "4" && existingList[3].FieldB2 == "D" &&
                          existingList[4].FieldB1 == "5");
        }


        class ClassA
        {
            public string FieldA1 { get; set; }
            public string FieldA2 { get; set; }
        }
        class ClassB
        {
            public string FieldB1 { get; set; }
            public string FieldB2 { get; set; }
        }

        class ClassBRefreshable : RefreshableModel<ClassA>
        {
            public string FieldB1 { get; set; }
            public string FieldB2 { get; set; }

            public override void Refresh(ClassA model)
            {
                FieldB1 = model.FieldA1;
                FieldB2 = model.FieldA2;
            }
        }

    }
}
