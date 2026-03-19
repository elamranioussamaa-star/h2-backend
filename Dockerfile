# Use the ASP.NET Core runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

# Use the SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy the csproj and restore dependencies
COPY ["H2-Trainning/H2-Trainning.csproj", "H2-Trainning/"]
RUN dotnet restore "H2-Trainning/H2-Trainning.csproj"

# Copy exactly everything else and build the application
COPY . .
WORKDIR "/src/H2-Trainning"
RUN dotnet build "H2-Trainning.csproj" -c Release -o /app/build

# Publish the built app to /app/publish directory
FROM build AS publish
RUN dotnet publish "H2-Trainning.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Final stage: copy the published app and define the entry point
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# The app binds to $PORT dynamically via Program.cs
ENTRYPOINT ["dotnet", "H2-Trainning.dll"]
