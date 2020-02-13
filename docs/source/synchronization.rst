Synchronization
***************
In Nethermind we support 3 different types of synchronization with network.

Fast sync
^^^^^^^^^

Fast sync can be directly enabled via ``--Sync.FastSync Enabled`` parameter. This option 
is enabled by default in any network config without ``_archive`` or ``_beam`` suffixes.

Full archive
^^^^^^^^^^^^

Beam sync (experimental)
^^^^^^^^^^^^^^^^^^^^^^^^

Beam sync can be directly enabled via ``--Sync.BeamSync Enabled`` parameter. This option 
is currently available for ``Görli Testnet`` and it is enabled by default in a config file with 
``_beam`` suffix e.g. ``goerli_beam.cfg``
