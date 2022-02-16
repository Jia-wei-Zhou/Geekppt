# Create a VSTO project for PowerPoint Add-in

## Step One

Launch Visual Studio, select ```Create a new Project```, then select ```PowerPoint VSTO Add-in```, use ```.NET Framework 4.7.2```

## Step Two

In the tool bar, click ```Project``` -> ```Add Component``` -> ```Ribbon (Visual Designer)```, change name to ```Code.cs``` and click ```Add```.

## Step Three

Copy all the files (except ```README.md```) to project folder. Then in ```Solution Explorer```, right click the solution and select ```Add``` -> ```Existing Item...```, select ```Auxiliary.CodeEvaluation.cs```, ```Auxiliary.Naming.cs``` and ```Auxiliary.RGBColor.cs```.

## Note

Before closing Visual Studio, click ```Build``` -> ```Clean Solution```, otherwise the add-in will present in the PowerPoint.  

test case ppt is in the test_case folder. Whenever a new features works, it is recommended to add a test slide. 