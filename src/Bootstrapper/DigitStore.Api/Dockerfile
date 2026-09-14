FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Directory.Packages.props", "./"]

COPY ["src/Bootstrapper/DigitStore.Api/DigitStore.Api.csproj", "src/Bootstrapper/DigitStore.Api/"]
COPY ["src/Modules/Account/Account.Module/Account.Module.csproj", "src/Modules/Account/Account.Module/"]
COPY ["src/Modules/Account/Account.Contracts/Account.Contracts.csproj", "src/Modules/Account/Account.Contracts/"]
COPY ["src/Modules/Customer/Customer.Module/Customer.Module.csproj", "src/Modules/Customer/Customer.Module/"]
COPY ["src/Modules/Customer/Customer.Contracts/Customer.Contracts.csproj", "src/Modules/Customer/Customer.Contracts/"]
COPY ["src/Modules/Catalog/Catalog.Module/Catalog.Module.csproj", "src/Modules/Catalog/Catalog.Module/"]
COPY ["src/Modules/Catalog/Catalog.Contracts/Catalog.Contracts.csproj", "src/Modules/Catalog/Catalog.Contracts/"]
COPY ["src/Modules/Distribution/Distribution.Module/Distribution.Module.csproj", "src/Modules/Distribution/Distribution.Module/"]
COPY ["src/Modules/Distribution/Distribution.Contracts/Distribution.Contracts.csproj", "src/Modules/Distribution/Distribution.Contracts/"]
COPY ["src/Modules/Order/Order.Module/Order.Module.csproj", "src/Modules/Order/Order.Module/"]
COPY ["src/Modules/Order/Order.Contracts/Order.Contracts.csproj", "src/Modules/Order/Order.Contracts/"]
COPY ["src/Modules/Payment/Payment.Module/Payment.Module.csproj", "src/Modules/Payment/Payment.Module/"]
COPY ["src/Modules/Payment/Payment.Contracts/Payment.Contracts.csproj", "src/Modules/Payment/Payment.Contracts/"]
COPY ["src/Modules/Search/Search.Module/Search.Module.csproj", "src/Modules/Search/Search.Module/"]
COPY ["src/Modules/Search/Search.Contracts/Search.Contracts.csproj", "src/Modules/Search/Search.Contracts/"]

COPY ["src/Shared/Shared.Abstractions/Shared.Abstractions.csproj", "src/Shared/Shared.Abstractions/"]
COPY ["src/Shared/Shared.EntityFramework/Shared.EntityFramework.csproj", "src/Shared/Shared.EntityFramework/"]
COPY ["src/Shared/Shared.Infrastructure/Shared.Infrastructure.csproj", "src/Shared/Shared.Infrastructure/"]
COPY ["src/Shared/Shared.Application/Shared.Application.csproj", "src/Shared/Shared.Application/"]

RUN dotnet restore "src/Bootstrapper/DigitStore.Api/DigitStore.Api.csproj"

COPY . .
WORKDIR "/src/src/Bootstrapper/DigitStore.Api"
RUN dotnet publish "DigitStore.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "DigitStore.Api.dll"]