# English Version

# DevContainer Docker-Compose Generator

## Overview

This tool is designed to generate a `docker-compose` configuration for usage in the Visual Studio Code DevContainer environment. It takes existing `docker-compose.yml` and `docker-compose.override.yml` files and combines them to create a DevContainer-compatible configuration.

## Usage

1. **Download the Release:**
   - Go to the [Releases](https://github.com/yourusername/devcontainer-generator/releases) section.
   - Download the release for your operating system (Windows, Mac, or Linux).

2. **Run the Generator:**
   - Extract the downloaded release.
   - Run the `DevcontainerGen` executable.

3. **Follow the Prompts:**
   - Enter the path to your existing `docker-compose.yml`.
   - Enter the path to your existing `docker-compose.override.yml`.
   - The tool will generate a new `docker-compose.devcontainer.yml` file.

4. **Use the Generated Configuration:**
   - The generated file can now be used as your `devcontainer.json` configuration for Visual Studio Code.

## Example

```bash
$ ./DevcontainerGen

Enter the path to your docker-compose.yml: /path/to/your/docker-compose.yml
Enter the path to your docker-compose.override.yml: /path/to/your/docker-compose.override.yml

Generated docker-compose.devcontainer.yml successfully!
```

# Versão em Português

# Gerador de Docker-Compose para DevContainer

## Visão Geral

Esta ferramenta foi desenvolvida para gerar uma configuração `docker-compose` para uso no ambiente DevContainer do Visual Studio Code. Ela utiliza os arquivos existentes `docker-compose.yml` e `docker-compose.override.yml` para criar uma configuração compatível com o DevContainer.

## Utilização

1. **Baixe a Versão:**
   - Acesse a seção de [Releases](https://github.com/seunome/devcontainer-generator/releases).
   - Baixe a release para o seu sistema operacional (Windows, Mac ou Linux).

2. **Execute o Gerador:**
   - Extraia o conteúdo da release baixada.
   - Execute o executável `DevcontainerGen`.

3. **Siga as Instruções:**
   - Informe o caminho do seu arquivo `docker-compose.yml` existente.
   - Informe o caminho do seu arquivo `docker-compose.override.yml` existente.
   - A ferramenta gerará um novo arquivo `docker-compose.devcontainer.yml`.

4. **Utilize a Configuração Gerada:**
   - O arquivo gerado pode ser usado como sua configuração `devcontainer.json` no Visual Studio Code.

## Exemplo

```bash
$ ./DevcontainerGen

Informe o caminho para o seu docker-compose.yml: /caminho/para/seu/docker-compose.yml
Informe o caminho para o seu docker-compose.override.yml: /caminho/para/seu/docker-compose.override.yml

Arquivo docker-compose.devcontainer.yml gerado com sucesso!
```
