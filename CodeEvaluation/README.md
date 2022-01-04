# Please use cmake to build this project

## For Visual Studio
Use following commands
```
cmake -G "Visual Studio 16 2019" ..
cmake --build .
```
Then launch ```CodeEvaluation.sln```.
In ```Solution Explorer```, right click ```CodeEvaluation```
and select ```Set as StartUp Project```.

## For MinGw
Use following commands
```
cmake -G "MinGW Makefiles" ..
mingw32-make
```

## For Xcode
Use following commands
```
cmake -G "Xcode" ..
cmake --build .
```
