# DSPair - Windows Version

<img src="./img/banner.jpg" alt="Banner DSPair">

> Conecte seu DualShock 4 com facilidade ao Windows.

## Sobre o projeto

DSPair é um projeto simples, em desenvolvimento, feito para solucionar demandas de projetos que implementam controles DS4 que necessitam de pareamento automático no SO Windows.
De forma simples e direta, você pode parear e desparear controles próximos ou através de seu MAC, iniciando também o serviço HID para comunicação com o sistema.

### Recursos

- [x] Parear via MAC Address
- [x] Desparear via MAC Address
- [x] Parear todos os dispositivos compatíveis próximos
- [x] Desparear todos os dispositivos compatíveis já pareados

### Ajustes e melhorias

O projeto ainda está em desenvolvimento e próximas atualizações serão voltadas nos seguintes recursos:

- [ ] Retornar lista de dispositivos encontrados (definindo melhor solução para expor esses dados; talvez API?)
- [ ] Retornar a lista de dispositivos já pareados
- [ ] Bugs menores...

## ☕ Usando o DSPair

Para utilizar o DSPair, você precisa iniciar o seu executável (disposnível em sua pasta `./bin`).
Note que a aplicação se baseia em modos de execução e é necessário entrar com informações via argumento.
Os estados do programa são disponibilizados por meio dos status de saída da execução da aplicação.

### Comandos do chat

> `DsPair.exe -a` - Busca todos os dispositivos compatíveis e realiza o pareamento de cada um.

> `DsPair.exe -c` - Busca todos os dispositivos já pareados compatíveis e realiza o despareamento de cada um.

> `DsPair.exe -p 'MAC ADDRESS AQUI'` - Pareia um dispositivo compatível a partir de seu endereço MAC.

> `DsPair.exe -u 'MAC ADDRESS AQUI'` - Despareia um dispositivo compatível a partir de seu endereço MAC.

### Status de saída

#### Status de sucesso

- Dispositivo pareado - `1`
- Dispositivo despareado - `2`
- Todos os dispositivos encontrados foram pareados - `3`
- Dispositivos encontrados parcialmente pareados - `4`
- Todos os dispositivos despareados - `5`
- Dispositivos parcialmente despareados - `6`

#### Status de falha

- Dispositivo pareado - `1`
- Dispositivo despareado - `2`
- Todos os dispositivos encontrados foram pareados - `3`
- Dispositivos encontrados parcialmente pareados - `4`
- Todos os dispositivos despareados - `5`
- Dispositivos parcialmente despareados - `6`

## 🤝 Reconhecimentos aos colaboradores

* [Daniel Amaral](https://github.com/danamaral92)
* [Gabriel Trindade](https://github.com/GabrielTrindade31)

## 🙋🏾‍♂️ Autor

* [Igor Oliveira](https://github.com/reedbluue) - Just another person

## 📝 Licença

Esse projeto está sob licença. Veja o arquivo [LICENÇA](./LICENSE) para mais detalhes.