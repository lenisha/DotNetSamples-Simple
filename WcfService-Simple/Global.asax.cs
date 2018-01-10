﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Pivotal;

namespace WcfService_Simple
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.Add(new ServiceRoute("api", new WebServiceHostFactory(), typeof(Service2)));

			String cert = "MIIMkAIBAzCCDEwGCSqGSIb3DQEHAaCCDD0Eggw5MIIMNTCCBdYGCSqGSIb3DQEHAaCCBccEggXDMIIFvzCCBbsGCyqGSIb3DQEMCgECoIIE/jCCBPowHAYKKoZIhvcNAQwBAzAOBAihy6mj5yP6VgICB9AEggTYaUVlIeMJF0/HONHnmjb9vi1wiZqS6zeyGdW8XZZFF3V7hYlR6qXha9bpir83ozkAGxwg+Llyj3jErmdM0qjzrtZADxgcxye0hWzFmKq8icJkliGfYeVJ3eEgm0Ag3f7TGmL/cLu1CaTp8u+g2MW4AH6lfeeyazMj9Zdqax6S6IAR28/nCZMYYbsN6qw3YJ6wElMFqDdAbkT8pEmlbn3qgP7VdnbxLVVQAmsqKv1zzvq0m2KZUOA9cpO/+FToMEG5XcJGu//mdqRWCb+Hx5SBdE+gyEk/ha47lVX380VkSmH0dqQlvVM7h4ZmhrHH6lQ5i7MML7lu8mZUknzrI2hVrgFkJMpRbTiB2FQkDfRpfatfYXXN0xOHRjMPz2yY4O9lUnLXcqQTja6Na7Ca2urcc7L+UBVC5u4kgdZz8pDBQ3QBm00ip7QIZkfhVucf1vqFATzTOIlJYGJiqoib4Js+fLv/UUB8fKAle8aEtnHj4PQ06ZLLjsylegPK/3B0YhACqVXRzECq8N+jXHB9rC/7kWDKSq+BnXBGjW6t36Gq9VzlItkSV8u13cf4LryoCi01TIS/MNEdsyyynp5hCAHE6u/Hj+r6GMqoTLWiiAi1kzvwSBZ+2rDigFxGGkgaDcPslThPWuGKcZlGexTd2/nQKu/zaNBjf6w3wUTt9YJUFZcJmBfclXL9HL9LQ4qfnaS6j4T/IM4p0C7hdPQkrhZKI84dxQuckHZxGage9JC/fpI/Tr8LaeS1B/OA/3qAtxLpnOOmMpAQiZ/LVLqnWxciLMUQhLn5/JKw/AIX81y5wO2Ol9CT5MfRF/RiazuKlRHO5QyPogUimJxF/kA5k9H7/LTKYB44A2HXENlekJpDO/hR3HYItLjSIAi6EfsGsa52afyjhOpybfahikam/hXlpJKP5mETyPJZNYfvqHuAGz/vHqpS534/tLk55bYV1By8X/4OjzXdaGKdrWtGsAjL2jImWgiOb0BD7J3R8ky2NvG9HpoZgZ+tiXJkdUvA8MmDtWOM4IrwBR0yjWpEXvnUyHlQ4tj4Qwnq/AYMoxJ2xIvsaPYEtFn5FLMts2n/JYNiRLKNX1pOOkiKB9N625LQUW5i56pwYTz5WguNwqxnTgSW7UolTOw9Kfw6zUs4WfSIlZUqmFcMijaqt4IRyKLcvNsHJt2vKIcKyFSEwv4R4B21W9PTX+J6I5szN/QNvz4R4BdVjc1HNNeG7N+bjQdLhVB+D10b9qcNSc1EKaDnyMtsrbFIAH7+U5ohH8KhGsRIN+L9bl0ZR1SqU+x+dMPjGqINMKgHxoBLK9q19kwDWSTOJREEbQxi1Fb753V5lcn83FRV6GVOd2riYcvP2I3qntXfSEht5/NUUpFy0X/P7YM7VteI+qvvI8WtZuPoTRR4jqIgDY8XvN/10vym7BhpS9LhUxOfa/EkIGOROffocMoGGyuXBYwLzFHHXdmbpUjNpvospvcDMPgq9tWW0BhuNUn7/ZIqFacr5VJHR7MMOu1ouPMIMvz8OmiXMPc7uTcDrvs9iU/L+O+PKEA9e7e2nTp7ojmxCOEFXfbNfblcDusWQXbIEDWCYOBOhqPjNsszqibIRmAdjXmBjLkCjQAT52ghinr88T4pnMpxAO2BaO+C0O45qQTvjTGBqTATBgkqhkiG9w0BCRUxBgQEAQAAADAzBgkqhkiG9w0BCRQxJh4kADMAMAAwADgAMgAwADEANgAwADMANAAwADAAMwA5ADUAMAAxMF0GCSsGAQQBgjcRATFQHk4ATQBpAGMAcgBvAHMAbwBmAHQAIABTAHQAcgBvAG4AZwAgAEMAcgB5AHAAdABvAGcAcgBhAHAAaABpAGMAIABQAHIAbwB2AGkAZABlAHIwggZXBgkqhkiG9w0BBwagggZIMIIGRAIBADCCBj0GCSqGSIb3DQEHATAcBgoqhkiG9w0BDAEGMA4ECFI4yDY2mb4hAgIH0ICCBhD/ymvqXkMUivDLOMrXqLGaCG1SJEgi2zJO1iDUoL+d1DGOgG06zjSLas9yjh5oxVIauyaCvkHjy88Mm5ET3KkxaI+EM/aZNOLGc7Uky86h+XC1VsLDY1neIa5oYPrLZM9WFYUc/UTcIRyxDDileB9A4h6vJiG14LxrdSqShykxBv+oOYsbsrgl/7ILZMxemXj/vDYP/PFIwB45eIrEf/OyZP0a0728rDcvLDGyRXFPn0l3lls+Y+GL43dVX35PPoE1yd1qHbwgy1ySpj5cIfk+GP1Rg6Ek/v5TKHIYLzD3zo5ZypswG/aFO7zU/xaZ1wGKA//umuH2DoxPYev1tTaxMMLHvaTBen33MUCvJMdGwTok447fgl9/V7ufuDn0EKdUdwLQx6GHKHgJTM91DN/DSyTfFWBo9lQALsfzWLq/oBxZqT1/itXfEXfZgHucVIp78WLEjxYv0O9zEntIA44PKKksNboY9rHlVgkMUU/Dw11VEKBhxO4mWAcswWkcIYA9buEEbOu7jhbethnk5QH+9sShGXumpZYbYqB5Tez0hkTRF+2DAUGvTu9gSVxDj0qILNBzuGIYyoQ1z2pVNeDVfJAML9g6PsUT9gV1KRk+AIXdaQV58zyYW0l1FPA+H/l+o3xldoYsgcNjPwudFFDQ1DhUiuXVSyCWaR104YCM+1fVR7O8z9g1Y9q4/uWnwEAtIY/9DSAifZjx4+TfkUgIlUIFLWG1KIeqjCNZOZEcDB/z140I0jJet/VF0he4h4NtdiST8mnGdRby4LHmmGtvRIhiDvdEjPp0WRaPGntfPYKGaSP/a9HxNU9ZPiHCDmsfyLholglTwqAJLllmveyHbuxmrSxM+TXWtuQw5AKG67H1td1DJhTnHWKUL4Sngp/r12w6Ww1BynP9ponGd/MwYL41xNrq9K0H3JwZSYt4Psrt1h0I7BuyDizK3DpByfxQBo3pv6kVMFw7D5K1vDOvwcc1VnF5132RoVrMwojJDKeY4IvQ6ZfOjNsDnFy/zIn0ccR/sig6ekLRfM1LUasUj9FcxlgfCYW1sE0m+Gi1n8oIdrZrNux7fwfREqQjLePxc8nA/bdFzWmx0kVtivOv3vLSvHXa0GG7Ee8oEypsliPu0dF7a99852OpOUJE2l7JMhamgyET4rIhh+iSIh25IqTpcYci8ihh9bmQuzj4okiDxLyLXQtUxnZaPe8DHrGOp4JJBlFkXG3Nuf85JK6A0UuJ1Mzid3SnpyWgEwsUJJVcENGqGumE9P3fygQMmdjxIssdedIzQR0wyZMl26/EHMmlahaLTO75RlBpfwiiPiDctvbjzkmNsmNMqss9NyVfn+8KUO98ORmmGkr+rWb9Lr8IVBbnCnLusefYj2zL9IgUmj8RZB3cB4LeXx6JanfJKF3evOHHgVkjsTVGYJaevW7fGbZnGEN9lljEkDxF4zHv5KUiFf1ODrV9kACiMONSlbKpTWfH0MSsJAbJqXqt/+EgEzNlzIpkYAIuYwyE/8LCn/M5bzMvQ7K2EITa/dMvVzIpYGnwNtrXu8eVHi8nkUzvY4Ikdj7Q4mIBMfLu1KzpgiS/lUJoiAQYJ6SJJGNkcVCZTaNdbM/6OOVWd0lwdA4l+94cllFbRGrarucdE9ksJxNCEDbu50ThXwPEC2/jRa+f0YrquLPa3oVHJTuD4rVMxj1zGGQz3JclX8zAXXUu4t2s8DYwmTYfwMgLIfDI8FM+4vfQIpwMP26BCO7j+staf2RnqaY9My4cgmHPk4bJ+jkXd1gEg28hmztwIvlp7ARynoGuCtIiughW+z+pLwnje9npbXML0bBl1IRRUkRjxHzxV5f04k73eeEeSvBZhZeG4TCFNARke+m3XlHgGaRz1MPBNZOy5Na/1I30xOzz0UHhOz5QJh4gIQHrJp3NhpMjFnV32qYgdk4WKL2HPT8Rnm/qH/aEyXg8TkEnDq+aWnd87vkRvCk3iv1EYxa1931n9ZgKhKv6vQT3i1NB/T38Qra4IYTa9zUa8AbqLtJ7J545511W0IfO2W3taFhNVW68RX8NC6+qdoVGoyd9MDswHzAHBgUrDgMCGgQUgE2KiHxMsjqrMs0bSBgN5Me1IjsEFOm4XXGmSYWXKKEta5wihcBeBjrkAgIH0A==";
			String passw = "test123";
			X509Certificate2 xcert = new X509Certificate2(Base64Methods.DecodeCert(cert), passw,
					X509KeyStorageFlags.PersistKeySet);

			Console.WriteLine("Adding certificate to the store {0}", xcert.GetName());
			X509Store store = new X509Store( StoreLocation.CurrentUser);
			store.Open(OpenFlags.ReadWrite);
			store.Add(xcert);
			store.Close();

			Console.WriteLine("Adding to MY store");

		}

		protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}