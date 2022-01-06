﻿namespace VeilleConcurrentielle.EventOrchestrator.ConsoleApp
{
    public class AppTerminator : IAppTerminator
    {
        public void Terminate(int exitCode)
        {
            Environment.Exit(exitCode);
        }
    }
}
