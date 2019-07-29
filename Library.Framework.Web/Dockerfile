From 192.168.1.6:5000/mcr.microsoft.com/dotnet/core/aspnet:2.2
RUN mkdir /app
RUN mkdir /app/Lib
RUN mkdir /app/Doc
RUN mkdir /app/Plugins
COPY ./Library.Framework.Web.dll /app/
WORKDIR /app
RUN chmod +x  Library.Framework.Web.dll
