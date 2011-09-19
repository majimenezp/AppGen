using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;

namespace NancyAppGenerator
{
    public class MigratorExecuter
    {
        string assemblyPath;
        string DataBaseType;
        string connStr;
        string currentPath;
        public MigratorExecuter(string assemblyPath,string DbType,string ConnectionString,string currentPath)
        {
            this.assemblyPath = assemblyPath;
            this.DataBaseType = DbType;
            this.connStr = ConnectionString;
            this.currentPath = currentPath;
        }
        public void StartMigration()
        {            
             var consoleAnnouncer = new TextWriterAnnouncer(System.Console.Out)
                                        {
                                            ShowElapsedTime = true,
                                            ShowSql = true
                                        };
             var runnerContext = new RunnerContext(consoleAnnouncer)
             {
                 
                 Database= DataBaseType,
                 Connection=connStr,
                 Target=assemblyPath,
                 PreviewOnly= false,
                 Namespace=string.Empty,
                 Task=string.Empty,
                 Version=0,
                 Steps=1,
                 WorkingDirectory = currentPath,
                 Profile=string.Empty,
                 Timeout=0,
                 ConnectionStringConfigPath=string.Empty
             };
             TaskExecutor tskMigration = new TaskExecutor(runnerContext);
             tskMigration.Execute();
        }

    }
}
