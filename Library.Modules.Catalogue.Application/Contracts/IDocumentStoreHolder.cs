﻿using Raven.Client.Documents.Session;
using Raven.Client.Documents.Subscriptions;

namespace Library.Modules.Catalogue.Application.Contracts;

public interface IDocumentStoreHolder : IDisposable
{
    IAsyncDocumentSession OpenAsyncSession();
    IAsyncDocumentSession OpenAsyncSessionWithOptimisticConcurrencyControl();
    SubscriptionWorker<T> GetSubscriptionWorker<T>(SubscriptionWorkerOptions options) where T : class;
}