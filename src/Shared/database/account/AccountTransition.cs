using System;
using System.Collections.Generic;

namespace Ava1.shared.database.account
{
    public class AccountTransition
    {
        private static Dictionary<string, Account> Tickets = new Dictionary<string, Account>();

        public static void addAccount(Account account, string ticket)
        {
            lock (Tickets)
            {
                try
                {
                    if (Tickets.ContainsKey(ticket) || ticket.Length < 17)
                        return;
                    Tickets.Add(ticket, account);
                }
                catch (Exception ex)
                {
                }
            }
        }

        public static Account getAccount(string ticket)
        {
            lock (Tickets)
            {
                try
                {
                    Account account = null;
                    if (Tickets.ContainsKey(ticket))
                    {
                        account = Tickets[ticket];
                        Tickets.Remove(ticket);
                    }
                    return account;
                }
                catch (Exception ex)
                {
                    
                }
            }
            return null;
        }
    }
}
