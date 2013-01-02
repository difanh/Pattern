:: Input mesh: 20x20C_packed.srm (an intermediate mesh that is a reasonably
:: sized, ~uniform sampling of the original domain)

:: First step: create BCs
:: Input is series of commands of the form:
::   --insertbc pos_x pos_y scope_radius m_11 m_12 m_22 --insertbc ...
tensorquad --geom 20x20C_packed.srm --bgmesh __  ^
  --insertbc  0.0  0.0   2.5    2.0  0.0  0.1    ^
  --insertbc -9.5 -9.5   2.5    2.0  0.0  2.05   ^
  --insertbc  9.5 -9.5   2.5    2.0  0.0  2.05   ^
  --insertbc -9.5  9.5   2.5    0.1  0.0  0.105  ^
  --insertbc  9.5  9.5   2.5    0.1  0.0  0.105  ^
  --savebgmesh insert.srf --savefield insert.nt2

:: Second step: generate field and export
tensorquad --geom insert.srf --disablepadbgmesh --bgmesh __ ^
  --field insert.nt2                                        ^
  --enablelogeuclideaninterp --enablepseudotensors          ^
  --genfield 1e-6 --genexteriorfield                        ^
  --savefield tensor.nt2 --savetfdlattice tensor.tfd 100 100

:: Outputs:
:: 1) tensor.tfd: to provide to VolMesh
:: 2) insert.srf and tensor.nt2: mesh containing vertices and file containing
:: tensor data on vertices

:: Test: meshes using the produced TFD
volmesh --BUB --PBUB -infile 20x20C_packed.srm -tfd tensor.tfd ^
  -remeshqualitymode -out test.srm -e 1200 -f 400

volmesh --BUB --PBUB -infile 20x20C_packed.srm -tfd tensor.tfd ^
  -remeshqualitymode -qdom -out test_qdom.srm -e 1200 -f 400
