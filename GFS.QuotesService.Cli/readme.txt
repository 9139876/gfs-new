Шаг 0:
    - переименовать исполняемый файл
        mv GFS.QuotesService.Cli ./qmcli

Запуск приложения по имени (без указания пути):

    - до перезапуска
        export PATH=$PATH:[path/to/program] 
    
    - насовсем
        gedit ~/.bashrc
        изменить/добавить export PATH=$PATH:[path/to/program] 
        source ~/.bashrc
        
Автодополнение команд:
    sudo cp qmcli_completion /etc/bash_completion.d/