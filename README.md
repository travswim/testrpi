# Testing project for the weathe station

* [ ] Talk about installation of Ubuntu
* [ ] Talk about installation of dotNET core 3.1 arm64
* [ ] Talk about enabling I2C for non-root user
* [ ] Talk about testing with python3
* [ ] Talk about setup on machine, deploy to remote

# Running the project
1. Publish
```powershell
> dotnet publish -r ubuntu-arm64
```
2. TCP transfer from machine to RPI
```powershell
> scp .\bin\Debug\netcoreapp3.0\ubuntu-arm64\publish\* ubuntu@192.168.86.179:testrpi
```
3. On the raspberry pi
```bash
$ sudo chmod 755 ./testrpi
$ ./testrpi
```
