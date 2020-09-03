# Not sure but this command might be for final release
# dotnet publish -c Release -r linux-arm -o ../publish testrpi.csproj

# Build on ubuntu-arm64
dotnet publish -r ubuntu-arm64
scp .\bin\Debug\netcoreapp3.1\ubuntu-arm64\publish\* ubuntu@192.168.86.179:testrpi