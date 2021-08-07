docker run --rm -ti `
    -e 'ACCEPT_EULA=Y' `
    -e 'SA_PASSWORD=l0cal!PW' `
    -p 1433:1433 `
    -v "$($(pwd).ToString().Replace('\', '/'))/data:/var/opt/mssql/data" `
    mcr.microsoft.com/mssql/server:2019-latest
