FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY bin/Release/net5.0 App/
WORKDIR /App
EXPOSE 8888
ENTRYPOINT ["dotnet", "REST.dll"]