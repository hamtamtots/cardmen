﻿using System;

namespace Cardmen.Messages.Commands
{
    public class BaseArticleOperationCommand
    {

        public Guid ArticleId { get; set; }


        public string OperationKey { get; set; }
    }
}
