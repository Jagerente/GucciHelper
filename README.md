"# GucciHelper" 

# dotnet Installation
* Create `.dotnet` folder:
```
$ mkdir ~/.dotnet
```
* Download [latest stables](https://dotnet.microsoft.com/download/dotnet/5.0)
* Extract archives to the `.dotnet` folder:
```
$ tar -xvf {ARCHIVE_NAME}.tar.gz -C ~/.dotnet
```
* Go to the `.bashrc`:
```
$ nano ~/.bashrc
```
* Add the lines below to the end of the .bashrc file:
```
export DOTNET_ROOT=$HOME/.dotnet 
export PATH=$PATH:$HOME/.dotnet
```
