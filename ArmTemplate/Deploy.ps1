#Az module requires requires at least following powershell modules installed: (Az.Resources, Az.Accounts)

param(
    [switch] $useParameters = $False,
    [parameter(Mandatory = $false)]
    [string] $subscriptionId
)

Connect-AzAccount

if (![string]::IsNullOrEmpty($subscriptionId))
{
    Set-AzContext -SubscriptionId $subscriptionId
}

if ($useParameters)
{
    New-AzSubscriptionDeployment -Location 'westeurope' -TemplateFile .\template.json -TemplateParameterFile .\template.parameters.json
}
else
{
    New-AzSubscriptionDeployment -Location 'westeurope' -TemplateFile .\template.json
}