using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace FeatureDealer.Models
{
    internal class FeaturesDBConnection : IConnection
    {
        private static DbContext Context;

        private static FeaturesDBConnection instance;

        private FeaturesDBConnection()
        {
            Context = new FeaturesContext();
        }

        public static FeaturesDBConnection GetInstance()
        {

            if (instance == null)
            {
                instance = new FeaturesDBConnection();
            }
            return instance;
        }

        public DbContext GetContext
        {
            get { return Context; }
        }
    }
    internal class EvalsDBConnection : IConnection
    {
        private static DbContext Context;

        private static EvalsDBConnection instance;

        private EvalsDBConnection()
        {
            Context = new EvaluationContext();
        }

        public static EvalsDBConnection GetInstance()
        {

            if (instance == null)
            {
                instance = new EvalsDBConnection();
            }
            return instance;
        }

        public DbContext GetContext
        {
            get { return Context; }
        }
    }

    internal class BuhOnlineDBConnection : IConnection
    {
        private static DbContext Context;

        private static BuhOnlineDBConnection instance;

        private BuhOnlineDBConnection()
        {
            Context = new BuhonlineContext();
        }

        public static BuhOnlineDBConnection GetInstance()
        {

            if (instance == null)
            {
                instance = new BuhOnlineDBConnection();
            }
            return instance;
        }


        public DbContext GetContext
        {
            get { return Context; }
        }

    }

    internal interface IConnection
    {
        DbContext GetContext { get; }
    }
}
