﻿using PAQK.Domain.Aggregates.Site.Commands;
using PAQK.Platform.Dispatching.Interfaces;
using PAQK.Platform.Domain.Interfaces;

namespace PAQK.Domain.Aggregates.Site
{
    public class SiteApplicationService : IMessageHandler
    {
        private readonly IRepository<SiteAggregate> _repository;

        public SiteApplicationService(IRepository<SiteAggregate> repository)
        {
            _repository = repository;
        }

        public void Handle(CreateSite c)
        {
            _repository.Perform(c.Id, a => a.Create(c));
        }

        public void Handle(SetSiteSettings c)
        {
            _repository.Perform(c.Id, a => a.UpdateSettings(c));
        }

        public void Handle(StartScheduler c)
        {
            _repository.Perform(c.Id, a => a.StartScheduler());
        }

        public void Handle(StopScheduler c)
        {
            _repository.Perform(c.Id, a => a.StopScheduler(c.Restart));
        }
    }
}