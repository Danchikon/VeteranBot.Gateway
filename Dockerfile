FROM mcr.microsoft.com/dotnet/sdk:8.0 AS builder
WORKDIR /app

COPY . .

RUN dotnet restore "src/VeteranBot.Gateway.Api/VeteranBot.Gateway.Api.csproj"
RUN dotnet publish "src/VeteranBot.Gateway.Api/VeteranBot.Gateway.Api.csproj" -c Release -o /dist 

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runner
WORKDIR /app

COPY --from=builder /dist /app

ENTRYPOINT ["dotnet", "VeteranBot.Gateway.Api.dll"]