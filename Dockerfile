FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Messenger.Presentation/Messenger.Presentation.csproj", "Messenger.Presentation/"]
COPY ["Messenger.BusinessLogic/Messenger.BusinessLogic.csproj", "Messenger.BusinessLogic/"]
COPY ["Messenger.ApplicationServices/Messenger.ApplicationServices.csproj", "Messenger.ApplicationServices/"]
COPY ["Messenger.Data/Messenger.Data.csproj", "Messenger.Data/"]
COPY ["Messenger.Domain/Messenger.Domain.csproj", "Messenger.Domain/"]
RUN dotnet restore "Messenger.Presentation/Messenger.Presentation.csproj"
COPY . .
WORKDIR "/src/Messenger.Presentation"
RUN dotnet build "Messenger.Presentation.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Messenger.Presentation.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet Messenger.Presentation.dll
