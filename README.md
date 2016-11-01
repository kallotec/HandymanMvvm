
# HandymanMvvm

A set of useful tools for working with MVVM architectures in C#. This is a Portable Class Library (PCL).

### ObservableCollection<> extensions

~~~~
using HandymanMvvm.Collections;

...

var existingList = new ObservableCollection<SomeViewModel>
{
    new ClassB { Id = "1", Distance = 1.9 },
    new ClassB { Id = "2", Distance = 0.2 },
};
var freshList = new List<DataModel>
{
    new ClassA { Id = "1", Distance = 1.0 },
    new ClassA { Id = "2", Distance = 1.6 },
    new ClassA { Id = "3", Distance = 1.1 },
};

// Update and order collection

existingList.Refresh(freshList,
    matchCondition: ((from, to) => from.Id == to.Id),
    createAndUpdateAction: ((from, to) =>
    {
        to.Id = from.Id;
        to.Distance = from.Distance;
    }),
    orderBy: (to) => to.Distance);

// Update and order collection (if target model : RefreshableModel<>)
// (Automatically calls to.Refresh(from) internally)

existingList.RefreshModels(freshList,
    matchCondition: (from, to) => from.Id == to.Id,
    orderBy: (to) => to.Distance);

~~~~