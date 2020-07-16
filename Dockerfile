FROM nethermindeth/rocksdb:6.4.6 AS rocksdb
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build

COPY . .
RUN echo "@v3.12 http://dl-cdn.alpinelinux.org/alpine/v3.12/main/" >> /etc/apk/repositories && \
    apk add --no-cache git@v3.12 openssl-dev@v3.12 && \
    git submodule update --init src/Dirichlet src/rocksdb-sharp && \
    dotnet publish src/Nethermind/Nethermind.Runner -c release -o out && \
    git describe --tags --always --long > out/git-hash

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine


RUN echo "@v3.12 http://dl-cdn.alpinelinux.org/alpine/v3.12/main/" >> /etc/apk/repositories && \
    apk --no-cache --no-progress add gosu --repository http://dl-cdn.alpinelinux.org/alpine/edge/testing/ snappy-dev@v3.12 && \
    addgroup -g 1000 nethermind &&  \
    adduser -u 1000 -G nethermind -s /bin/sh -D nethermind

USER nethermind

WORKDIR /nethermind

COPY --from=build --chown=nethermind:nethermind /out .
COPY --from=rocksdb --chown=nethermind:nethermind /nethermind/librocksdb.so /nethermind/librocksdb.so

ARG GIT_COMMIT=unspecified
LABEL git_commit=$GIT_COMMIT

# COPY entrypoint.sh /nethermind/entrypoint.sh

ENTRYPOINT ["./Nethermind.Runner"]