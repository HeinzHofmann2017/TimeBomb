﻿using System;
using Raven.Client.Documents;
using Raven.Client.Exceptions;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;

namespace TimeBomb2.Data.Access
{
    public static class DocumentStoreHolder
    {
        private static readonly Lazy<IDocumentStore> LocalStore = new Lazy<IDocumentStore>(CreateDocumentStore);

        private static IDocumentStore CreateDocumentStore()
        {
            IDocumentStore documentStore = new DocumentStore
            {
                Urls = new[] {"http://localhost:8080"},
                Database = "TimeBomb"
            };

            documentStore.Initialize();
            return documentStore;
        }

        public static string CreateTimeBombDatabaseIfNotYetExists()
        {
            try
            {
                Store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord("TimeBomb")));
                return "Timebomb Database successfully created";
            }
            catch (ConcurrencyException)
            {
                return "Timebomb Database already exists";
            }
        }

        public static IDocumentStore Store => LocalStore.Value;
    }
}