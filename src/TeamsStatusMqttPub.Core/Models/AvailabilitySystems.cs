﻿using System.ComponentModel;

namespace TeamsStatusMqttPub.Core.Models;

/// <summary>
///     The different systems that this application can parse availability from.
/// </summary>
public enum AvailabilitySystems
{
    [Description("Teams Classic")] MicrosoftTeamsClassic,

    [Description("Teams")] MicrosoftTeams
}
