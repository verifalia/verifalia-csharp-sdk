/*
* Verifalia - Email list cleaning and real-time email verification service
* https://verifalia.com/
* support@verifalia.com
*
* Copyright (c) 2005-2019 Cobisi Research
*
* Cobisi Research
* Via Prima Strada, 35
* 35129, Padova
* Italy - European Union
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
*
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
*
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using Flurl.Http;

namespace Verifalia.Api.Security
{
    internal class UsernamePasswordAuthenticator : IAuthenticator
    {
        private readonly string _username;
        private readonly string _password;

        public UsernamePasswordAuthenticator(string username, string password)
        {
            if (String.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username),
                    "username is null or empty: please visit https://verifalia.com/client-area to set up a new user, if you don't have one.");
            }

            if (String.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException(nameof(password),
                    "password is null or empty: please visit https://verifalia.com/client-area to set up a new user, if you don't have one.");
            }

            _username = username;
            _password = password;
        }

        public IFlurlClient AddAuthentication(IFlurlClient flurlClient)
        {
            return flurlClient.WithBasicAuth(_username, _password);
        }
    }
}
